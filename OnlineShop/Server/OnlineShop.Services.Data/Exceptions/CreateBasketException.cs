namespace OnlineShop.Services.Data.Exceptions
{
    public class CreateBasketException : Exception
    {
        public CreateBasketException(string message) :base(message) 
        {
            
        }
        public CreateBasketException() : base("Error creating basket.")
        {
            
        }
    }
}
