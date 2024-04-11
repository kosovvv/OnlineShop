using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Data.Models;
using OnlineShop.Data.Models.Identity;
using OnlineShop.Services.Data.Helpers;
using OnlineShop.Services.Data.Interfaces;
using OnlineShop.Web.ViewModels.Review;
using System.Security.Claims;

namespace OnlineShop.Services.Data.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly StoreContext context;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewService(StoreContext context, IMapper mapper, IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.orderService = orderService;
            this.userManager = userManager;
        }

        public async Task<ReturnReviewDto> CreateReview(ClaimsPrincipal user, CreateReviewDto review)
        {
            var reviewToCreate = this.mapper.Map<CreateReviewDto, Review>(review);
            //to fix later
            var product = await this.context.Products.FirstOrDefaultAsync(x => x.Id == review.ReviewedProduct.Id);
            var author = await this.userManager.FindByEmailFromClaimsPrincipal(user);

            if (product == null || author == null)
            {
                return null;
            }
            var hasUserAlreadyReviewed = await this.HasUserAlreadyReviewedProduct(user, (int)product.Id);

            if (hasUserAlreadyReviewed)
            {
                return null;
            }

            reviewToCreate.Author = author;
            reviewToCreate.ReviewedProduct = product;
            reviewToCreate.IsVerified = await this.orderService.HasUserBoughtProduct(author.Email, review.ReviewedProduct.Id);

            await this.context.AddAsync(reviewToCreate);
            await this.context.SaveChangesAsync();

            return this.mapper.Map<Review, ReturnReviewDto>(reviewToCreate);
        }

        public async Task<bool> DeleteReview(int id)
        {
            var reviewToDelete = await this.context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

            if (reviewToDelete == null)
            {
                return false;
            }

            this.context.Reviews.Remove(reviewToDelete);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<ReturnReviewDto> EditReview(int id, CreateReviewDto review)
        {
            var reviewToEdit = await this.context.Reviews
                .Include(x => x.Author)
                .Include(x => x.ReviewedProduct)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (reviewToEdit == null) 
            {
                return null;
            }

            reviewToEdit.Description = review.Description;
            reviewToEdit.Score = review.Score;
            await this.context.SaveChangesAsync();

            return this.mapper.Map<Review, ReturnReviewDto>(reviewToEdit);
        }

        public async Task<ICollection<ReturnReviewDto>> GetReviewsByProduct(int productId)
        {
            var reviews = await this.context.Reviews.Include(x => x.ReviewedProduct).Include(x => x.Author)
                .Where(x => x.ReviewedProduct.Id == productId).ToListAsync();

            return this.mapper.Map<ICollection<Review>, ICollection<ReturnReviewDto>>(reviews);
        }

        public async Task<bool> HasUserAlreadyReviewedProduct(ClaimsPrincipal user, int productId)
        {
            var author = await this.userManager.FindByEmailFromClaimsPrincipal(user);
            return await context.Reviews.Include(x => x.Author).Include(x => x.ReviewedProduct)
                .Where(x => x.ReviewedProduct.Id == productId && x.Author.Email == author.Email).AnyAsync();
        }
    }
}
