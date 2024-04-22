namespace OnlineShop.Common.Entities
{
    public static class ProductConstants
    {
        public const string NameRequiredMessage = "Name is required.";

        public const string DescriptionRequiredMessage = "Description is required.";

        public const string PriceRequiredMessage = "Price is required.";
        public const double MinPrice = 0;
        public const double MaxPrice = 999999.99;
        public const string PriceRangeMessage = "Price must be greater than or equal to 0";

        public const string PictureUrlRequiredMessage = "Picture URL is required.";
        public const string PictureUrlInvalidFormatMessage = "Invalid URL format.";

        public const string ProductTypeIdRequiredMessage = "Product type ID is required.";

        public const string ProductBrandIdRequiredMessage = "Product brand ID is required.";
    }
}
