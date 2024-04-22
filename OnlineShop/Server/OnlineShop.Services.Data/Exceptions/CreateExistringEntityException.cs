namespace OnlineShop.Services.Data.Exceptions
{
    public class CreateExistringEntityException : Exception
    {
        public CreateExistringEntityException(string message) : base(message)
        {
            
        }
        public CreateExistringEntityException(Type type) : base($"This {type.GetType().Name} already exists")
        {
            
        }
    }
}
