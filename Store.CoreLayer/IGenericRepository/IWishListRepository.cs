using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IGenericRepository
{
    public interface IWishListRepository
    {
        Task<string?> AddAsync(string customerId, int productId);

        Task<bool> RemoveAsync(string customerId, int productId);

        Task<IReadOnlyList<WishList?>> GetWishlistAsync(string customerId,WishListSpecParamter paramter);

        Task<bool> ExistsAsync(string customerId, int productId);
    }
}
