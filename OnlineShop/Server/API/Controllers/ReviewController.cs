using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Review;

namespace OnlineShop.WebAPI.Controllers
{
    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;
        private readonly IProductService productService;

        public ReviewController(IReviewService reviewService, IProductService productService)
        {
            this.reviewService = reviewService;
            this.productService = productService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<ReturnReviewDto>> CreateReviewForProduct(CreateReviewDto reviewToCreate)
        {
            var createdReview = await this.reviewService.CreateReview(User,reviewToCreate);

            if (createdReview == null)
            {
                return BadRequest(new ApiResponse(400, "Error creating review"));
            }

            return Ok(createdReview);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ICollection<ReturnReviewDto>>> GetReviewsByProduct(int productId)
        {
            var product = await this.productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return BadRequest(new ApiResponse(400, "No such product"));
            }

            var reviews = await this.reviewService.GetReviewsByProduct(productId);
            return Ok(reviews);
        }

        [HttpGet("isReviewed/{productId}")]
        public async Task<ActionResult<bool>> IsProductAlreadyReviewdByUser(int productId)
        {
            return await this.reviewService.HasUserAlreadyReviewedProduct(User, productId);
        }
    }
}
