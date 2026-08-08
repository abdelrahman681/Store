using Microsoft.AspNetCore.Identity;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.CoreLayer.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ReviewService(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }
        public async Task<Review?> AddReviewAsync(int productId, int rating, string email, string? Comment = null)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            var user = await _userManager.FindByEmailAsync(email);
            var Spec = new ReviewSpec(user.Id,productId);
            var existingReview = await _unitOfWork.Repository<Review>().GetByIdAsyncWithSpecification(Spec);
            if (existingReview is  null)
            {
                var review = new Review
                {
                    ProductId = productId,
                    CustomerId = user.Id,
                    Rating = rating,
                    Comment = Comment,
                    CreatedAt = DateTime.Now
                };

                await _unitOfWork.Repository<Review>().AddAsync(review);

                var result = await _unitOfWork.CompleteAsync();
                if (result <= 0) return null;
                await UpdateProductRatingAsync(productId);
                return review;
            }
            return null;
        }

        public async Task<bool> DeleteReviewAsync(int id, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var spec = new ReviewSpec(id,user.Id);
            var review = await _unitOfWork.Repository<Review>().GetByIdAsyncWithSpecification(spec);
            if (review is not null)
            {
                _unitOfWork.Repository<Review>().Delete(review);
                var result = await _unitOfWork.CompleteAsync();
                await UpdateProductRatingAsync(review.ProductId);
                return true;
            }
            return false;
        }

        public async Task<IReadOnlyList<Review>> GetProductReviewsAsync(ReviewParameter parameter)
        {
            var spec = new ReviewSpec(parameter);

            var reviews = await _unitOfWork
                .Repository<Review>()
                .GetAllAsyncWithSpecification(spec);

            return reviews;
        }

        public async Task<Review?> GetReviewAsync(int id)
        {
            var review = await _unitOfWork.Repository<Review>().GetByIdAsync(id);
            return review;
        }

        public async Task<bool> UpdateReviewAsync(int id, int rating, string email, string? comment=null)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var spec = new ReviewSpec(id, user.Id);
            var review = await _unitOfWork.Repository<Review>().GetByIdAsyncWithSpecification(spec);
            if (review is not null)
            {
                review.Rating = rating;
                review.Comment = comment;
                review.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Repository<Review>().Update(review);

                var result = await _unitOfWork.CompleteAsync();
                await UpdateProductRatingAsync(review.ProductId);
                if (result > 0) return true;
            }
            return false;
        }
        private async Task UpdateProductRatingAsync(int productId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);

            if (product is null)
                return;
            var spec = new ReviewSpec(null, productId);
            var reviews = await _unitOfWork.Repository<Review>().GetAllAsyncWithSpecification(spec);     
            product.ReviewsCount = reviews.Count;
            product.AverageRating = reviews.Any()? reviews.Average(r => r.Rating): 0;
                
            _unitOfWork.Repository<Product>().Update(product);

            await _unitOfWork.CompleteAsync();
        }
    }
}
