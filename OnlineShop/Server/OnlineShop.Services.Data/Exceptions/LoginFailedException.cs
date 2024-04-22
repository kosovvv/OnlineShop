namespace OnlineShop.Services.Data.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException(string message) : base(message) { }
        public LoginFailedException() : base("Error logging in.")
        {
            
        }
    }
}
