namespace OnlineShop.Services.Data.Exceptions
{
    public class UpdateOrderFailedException : Exception
    {
        public UpdateOrderFailedException(string message): base(message) { }
    }
}
