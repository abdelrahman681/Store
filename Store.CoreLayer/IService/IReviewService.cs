using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IService
{
    public interface IReviewService
    {
        Task<Review?> AddReviewAsync(int productId,int rating, string email,string? Comment=null);

        Task<Review?> GetReviewAsync(int id);

        Task<IReadOnlyList<Review>> GetProductReviewsAsync(ReviewParameter parameter);

        Task<bool> UpdateReviewAsync(int id, int Rating, string email, string? Comment = null);

        Task<bool> DeleteReviewAsync(int id, string email);
    }
}
