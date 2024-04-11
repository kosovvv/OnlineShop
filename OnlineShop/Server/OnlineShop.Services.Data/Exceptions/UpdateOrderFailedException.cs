namespace OnlineShop.Services.Data.Exceptions
{
    internal class UpdateOrderFailedException : Exception
    {
        public UpdateOrderFailedException(string message): base(message) { }
    }
}
