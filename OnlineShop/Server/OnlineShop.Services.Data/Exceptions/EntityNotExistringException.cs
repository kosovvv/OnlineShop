namespace OnlineShop.Services.Data.Exceptions
{
    public class EntityNotExistingException : Exception
    {
        public EntityNotExistingException(string message): base(message)
        {
            
        }

        public EntityNotExistingException(Type type) : base ($"{type.GetType().Name} not found.")
        {
            
        }
    }
}
