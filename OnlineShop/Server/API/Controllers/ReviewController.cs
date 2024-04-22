using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.Infrastructure;
using OnlineShop.Web.ViewModels;
using OnlineShop.Web.ViewModels.Review;

namespace OnlineShop.WebAPI.Controllers
{
    public class ReviewController : BaseApiController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<ReturnReviewDto>> CreateReviewForProduct(CreateReviewDto reviewToCreate)
        {
            try
            {
                var createdReview = await this.reviewService.CreateReview(User.GetId(), reviewToCreate);
                return Ok(createdReview);
            }
            catch (InvalidEntityException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            catch (ReviewAlreadyExistsException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Cached(600)]
        public async Task<ActionResult<ICollection<ReturnReviewDto>>> GetReviewsByProduct(int productId)
        {
            try
            {
                var reviews = await this.reviewService.GetReviewsByProduct(productId);
                return Ok(reviews);
            }
            catch (EntityNotExistingException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<ReturnReviewDto>> EditReview(int reviewId, CreateReviewDto reviewToEdit)
        {
            try
            {
                var editedReview = await this.reviewService.EditReview(reviewId, reviewToEdit);

                if (editedReview == null)
                {
                    return BadRequest(new ApiResponse(400, "Failed to edit review"));
                }

                return Ok(editedReview);
            }
            catch (InvalidEntityException ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
            
        }
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult> DeleteReview(int reviewId)
        {
            var isDeleted = await reviewService.DeleteReview(reviewId);
            return isDeleted ? Ok() : NotFound();
        }

        [HttpGet("isReviewed/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<bool>> IsProductAlreadyReviewdByUser(int productId)
        {
            return await this.reviewService.HasUserAlreadyReviewedProduct(User.GetId(), productId);
        }
    }
}
