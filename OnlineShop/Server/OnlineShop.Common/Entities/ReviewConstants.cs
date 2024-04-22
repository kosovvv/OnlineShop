namespace OnlineShop.Common.Entities
{
    public class ReviewConstants
    {
        public const string ScoreRequiredMessage = "Score is required.";
        public const string ScoreRangeMessage = "Score must be between 1 and 10.";
        public const int MinScore = 1;
        public const int MaxScore = 0;
        public const string ReviewedProductIdRequiredMessage = "Reviewed product ID is required.";
        public const string DescriptionRequiredMessage = "Description is required.";
        public const int MaxDescriptionLength = 500;
        public const string DescriptionMaxLengthMessage = "Description cannot exceed 500 characters.";
    }
}
