namespace OnlineShop.Common.Entities
{
    public class BasketConstants
    {
        public const string PriceRangeMessage = "Price must be greater than zero.";
        public const double MinPrice = 0.1; 

        public const string QuantityRangeMessage = "Quantity must be at least 1.";
        public const int MinQuantity = 1;
    }
}
