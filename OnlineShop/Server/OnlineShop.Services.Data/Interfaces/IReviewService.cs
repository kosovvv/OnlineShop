using OnlineShop.Web.ViewModels.Review;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IReviewService
    {
        Task<ReturnReviewDto> CreateReview(string userEmail, CreateReviewDto review);
        Task<ReturnReviewDto> EditReview(int id, CreateReviewDto review);
        Task<bool> DeleteReview(int id);
        Task<ICollection<ReturnReviewDto>> GetReviewsByProduct(int productId);
        Task<bool> HasUserAlreadyReviewedProduct(string userEmail, int productId);
    }
}
