namespace OnlineShop.Services.Data.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(string fileName, Stream stream);
        Task<byte[]> GetImageAsync(string imageId);
    }
}
