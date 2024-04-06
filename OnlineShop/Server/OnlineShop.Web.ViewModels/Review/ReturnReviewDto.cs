namespace OnlineShop.Web.ViewModels.Review
{
    public class ReturnReviewDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public ProductToReturnDto ReviewedProduct { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsVerified { get; set; }
    }
}
