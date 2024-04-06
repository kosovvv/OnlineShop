using OnlineShop.Web.ViewModels.Review;

namespace OnlineShop.Services.Data.Interfaces
{
    internal interface IReviewService
    {
        Task<ReturnReviewDto> CreateReview(CreateReviewDto review);
        Task<ReturnReviewDto> EditReview(int id, CreateReviewDto review);
        Task<bool> DeleteReview(int id);
        Task<ICollection<ReturnReviewDto>> GetReviewsByProduct(int id);
        Task<bool> CheckIsReviewVerified(int id);
    }
}
