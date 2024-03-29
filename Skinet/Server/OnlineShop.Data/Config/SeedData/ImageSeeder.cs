using MongoDB.Bson;
using MongoDB.Driver;
using OnlineShop.Data.Models;

namespace OnlineShop.Data.Config.SeedData
{
    public static class ImageSeeder
    {
        private static readonly IMongoCollection<Image> _imageCollection;

        static ImageSeeder()
        {
            var database = GetMongoDatabase();
            _imageCollection = database.GetCollection<Image>("images");
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
            if (await _imageCollection.CountDocumentsAsync(new BsonDocument()) == 0)
            {
                var imageDirectory = @"../OnlineShop.WebAPI/wwwrooot/images/products";

                if (Directory.Exists(imageDirectory))
                {
                    var imagePaths = Directory.GetFiles(imageDirectory);

                    foreach (var imagePath in imagePaths)
                    {
                        var imageData = File.ReadAllBytes(imagePath);
                        var imageNameWithExtension = Path.GetFileName(imagePath);

                        var image = new Image
                        {
                            Name = imageNameWithExtension,
                            Data = imageData
                        };

                        await _imageCollection.InsertOneAsync(image);
                    }
                }
                else
                {
                    throw new DirectoryNotFoundException($"Directory '{imageDirectory}' not found.");
                }
            }
        }
    }
}