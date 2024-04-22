namespace OnlineShop.Services.Data.Exceptions
{
    public class SavingUserAddressException : Exception 
    {
        public SavingUserAddressException(string message) : base(message) { }

        public SavingUserAddressException() : base("Error saving email address")
        {
            
        }
    }
}
