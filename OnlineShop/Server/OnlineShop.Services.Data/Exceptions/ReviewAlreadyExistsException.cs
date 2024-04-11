namespace OnlineShop.Services.Data.Exceptions
{
    public class ReviewAlreadyExistsException : Exception
    {
        public ReviewAlreadyExistsException(string message) : base(message) { }
    }
}
