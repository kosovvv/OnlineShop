using MongoDB.Driver;
using OnlineShop.Data.Models;

namespace OnlineShop.Data.Seeding.Seeders
{
    public static class ImageSeeder
    {
        private static readonly IMongoCollection<Image> imageCollection;

        static ImageSeeder()
        {
            var database = GetMongoDatabase();
            imageCollection = database.GetCollection<Image>("images");
        }

        private static IMongoDatabase GetMongoDatabase()
        {
            var connectionString = "mongodb://localhost:27017";
            var databaseName = "OnlineShop";
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }

        public static async Task SeedImagesAsync()
        {
            var imageDirectory = @"../API/wwwroot/images";

            if (Directory.Exists(imageDirectory))
            {
                await SeedImagesFromDirectoryAsync(imageDirectory);
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory '{imageDirectory}' not found.");
            }
        }

        private static async Task SeedImagesFromDirectoryAsync(string directory)
        {
            var directories = Directory.GetDirectories(directory);
            foreach (var subDirectory in directories)
            {
                await SeedImagesFromDirectoryAsync(subDirectory);
            }

            var imagePaths = Directory.GetFiles(directory);
            foreach (var imagePath in imagePaths)
            {
                var imageData = File.ReadAllBytes(imagePath);
                var imageNameWithExtension = Path.GetFileName(imagePath);

                var image = new Image
                {
                    Name = imageNameWithExtension,
                    Data = imageData
                };

                await imageCollection.InsertOneAsync(image);
            }
        }

    }
}