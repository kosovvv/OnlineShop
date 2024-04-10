using OnlineShop.Web.ViewModels.Review;
using System.Security.Claims;

namespace OnlineShop.Services.Data.Interfaces
{
    public interface IReviewService
    {
        Task<ReturnReviewDto> CreateReview(ClaimsPrincipal user, CreateReviewDto review);
        Task<ReturnReviewDto> EditReview(int id, CreateReviewDto review);
        Task<bool> DeleteReview(int id);
        Task<ICollection<ReturnReviewDto>> GetReviewsByProduct(int productId);
        Task<bool> HasUserAlreadyReviewedProduct(ClaimsPrincipal user, int productId);
    }
}
