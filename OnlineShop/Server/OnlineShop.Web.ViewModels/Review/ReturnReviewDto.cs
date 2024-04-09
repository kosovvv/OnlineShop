namespace OnlineShop.Web.ViewModels.Review
{
    public class ReturnReviewDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public int Score {  get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsVerified { get; set; }
    }
}
