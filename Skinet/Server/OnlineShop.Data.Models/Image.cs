using MongoDB.Bson;

namespace OnlineShop.Data.Models
{
    public class Image
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
