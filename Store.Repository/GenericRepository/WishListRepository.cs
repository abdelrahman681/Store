using Microsoft.AspNetCore.Identity;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IUnitOfWork;
using Store.Repository.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.GenericRepository
{
    public class WishListRepository : IWishListRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public WishListRepository(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<string?> AddAsync(string customerId, int productId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
               

            if (product is null)
                return("Product not found.");
            var spec=new WishListSpec(customerId, productId);
            var wishlistItem = await _unitOfWork.Repository<WishList>().GetByIdAsyncWithSpecification(spec);
            

            if (wishlistItem is not null)
                return  ("Product already exists in wishlist.");

            var wishlist = new WishList
            {
                CustomerId = customerId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.Repository<WishList>().AddAsync(wishlist);
            var result = await _unitOfWork.CompleteAsync();
            if (result > 0) return "Addedd";
            return null;
        }

        public async Task<bool> ExistsAsync(string customerId, int productId)
        {
            var wishlistItem= (await _unitOfWork.Repository<WishList>().GetAllAsync())
                .FirstOrDefault(x=>x.CustomerId==customerId && x.ProductId == productId);
            if (wishlistItem is not null)
                return true;
            else return false;
        }

        public async Task<IReadOnlyList<WishList?>> GetWishlistAsync(string customerId,WishListSpecParamter paramter)
        {
            var spec=new WishListSpec(customerId,paramter);
            var wishlistItem = await _unitOfWork.Repository<WishList>().GetAllAsyncWithSpecification(spec);
            if (wishlistItem is null) return null;
            return wishlistItem;
        }

        public async Task<bool> RemoveAsync(string customerId, int productId)
        {
            var spec = new WishListSpec(customerId,productId);
            var wishlistItem = await _unitOfWork.Repository<WishList>().GetByIdAsyncWithSpecification(spec);
            _unitOfWork.Repository<WishList>().Delete(wishlistItem);
            var result= await _unitOfWork.CompleteAsync();
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
