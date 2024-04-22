namespace OnlineShop.Services.Data.Exceptions
{
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException(string message): base(message)
        {
            
        }
        public InvalidEntityException(Type type) : base($"Invalid {type.GetType().Name}")
        {
            
        }
    }
}
