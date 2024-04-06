using MongoDB.Driver;
using OnlineShop.Data.Models;
using OnlineShop.Services.Data.Interfaces;

namespace OnlineShop.Services.Data.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IMongoCollection<Image> imageCollection;

        public ImageService(IMongoDatabase database)
        {
            imageCollection = database.GetCollection<Image>("images");
        }

        public async Task<byte[]> GetImageAsync(string imageName)
        {
            var filter = Builders<Image>.Filter.Eq("Name", imageName);
            var image = await imageCollection.Find(filter).FirstOrDefaultAsync();

            return image?.Data;
        }


        public async Task<string> UploadImageAsync(string fileName, Stream stream)
        {
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var image = new Image
            {
                Name = fileName,
                Data = imageData
            };

            await imageCollection.InsertOneAsync(image);
            return image.Id.ToString();
        }
    }
}
