using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.Common;
using OnlineShop.Data.Common.Repositories;
using OnlineShop.Data.Models;
using OnlineShop.Services.Data.Exceptions;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels.Review;

namespace OnlineShop.Services.Data.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Review> reviewRepository;
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IOrderService orderService, IProductService productService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.orderService = orderService;
            this.reviewRepository = this.unitOfWork.GetRepository<Review>();
            this.productService = productService;
        }

        public async Task<ReturnReviewDto> CreateReview(string userId, CreateReviewDto review)
        {
            var reviewToCreate = this.mapper.Map<CreateReviewDto, Review>(review);
            var product = await this.productService.GetProductByIdAsync(review.ReviewedProduct.Id);

            if (product == null || userId == null)
            {
                throw new InvalidEntityException(typeof(Review));
            }
            var hasUserAlreadyReviewed = await this.HasUserAlreadyReviewedProduct(userId, (int)product.Id);

            if (hasUserAlreadyReviewed)
            {
                throw new ReviewAlreadyExistsException("The user already reviewed this product");
            }

            reviewToCreate.AuthorId = userId;
            reviewToCreate.ReviewedProductId = product.Id;
            reviewToCreate.IsVerified = await this.orderService.HasUserBoughtProduct(userId, review.ReviewedProduct.Id);

            await this.reviewRepository.AddAsync(reviewToCreate);
            await this.unitOfWork.Save();

            return this.mapper.Map<Review, ReturnReviewDto>(reviewToCreate);
        }

        public async Task<bool> DeleteReview(int id)
        {
            var reviewToDelete = await this.reviewRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (reviewToDelete == null)
            {
                return false;
            }

            this.reviewRepository.Delete(reviewToDelete);
            await unitOfWork.Save();
            return true;
        }

        public async Task<ReturnReviewDto> EditReview(int id, CreateReviewDto review)
        {
            var reviewToEdit = await this.reviewRepository.All()
                .Include(x => x.Author)
                .Include(x => x.ReviewedProduct)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reviewToEdit == null) 
            {
                throw new EntityNotExistingException(typeof(Review));
            }

            reviewToEdit.Description = review.Description;
            reviewToEdit.Score = review.Score;

            this.reviewRepository.Update(reviewToEdit);
            await this.unitOfWork.Save();

            return this.mapper.Map<Review, ReturnReviewDto>(reviewToEdit);
        }

        public async Task<ICollection<ReturnReviewDto>> GetReviewsByProduct(int productId)
        {
            var reviews = await this.reviewRepository.All().Include(x => x.ReviewedProduct).Include(x => x.Author)
                .Where(x => x.ReviewedProduct.Id == productId).ToListAsync();

            return this.mapper.Map<ICollection<Review>, ICollection<ReturnReviewDto>>(reviews);
        }

        public async Task<bool> HasUserAlreadyReviewedProduct(string userId, int productId)
        {
            var result = await reviewRepository.All().Include(x => x.Author).Include(x => x.ReviewedProduct)
                .Where(x => x.ReviewedProduct.Id == productId && x.Author.Id == userId).AnyAsync();
            return result;
        }
    }
}
