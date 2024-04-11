namespace OnlineShop.Services.Data.Exceptions
{
    public class ProductNotExistingException : Exception
    {
        public ProductNotExistingException(string message): base(message)
        {
            
        }
    }
}
