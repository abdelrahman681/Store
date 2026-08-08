using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.ISepecfication;
using Store.DTO;
using Store.Errors;
using Store.Helpers;
using System.Security.Claims;

namespace Store.Controllers
{

    public class WishListController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishListRepository _listRepository;
        private readonly IMapper _mapper;

        public WishListController(UserManager<AppUser> userManager,
            IWishListRepository listRepository,IMapper mapper)
        {
            _userManager = userManager;
            _listRepository = listRepository;
            this._mapper = mapper;
        }
        [Authorize]
        [HttpPost("AddProductToWishList/{ProductId}")]
        public async Task<ActionResult<string>> AddProductToWishList(int ProductId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var result=await _listRepository.AddAsync(user.Id,ProductId);
            if (result == "Product already exists in wishlist.")
                return BadRequest(new ErrorApiResponse(401, "Product already exists in wishlist."));
            return Ok("Product added to wishlist successfully.");
        }
        [Cache(30)]
        [Authorize]
        [HttpGet("GetAllWishList")]
        public async Task<ActionResult<IReadOnlyList<Pagination<WishListDTO>>>> GetAllWishList(WishListSpecParamter paramter)
        {
            //var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (string.IsNullOrEmpty(user.Id))
                return Unauthorized();
            var wishlistItem=await _listRepository.GetWishlistAsync(user.Id,paramter);
            if(wishlistItem is null) return NotFound(new ErrorApiResponse(404));
            var MappedList = _mapper.Map<IReadOnlyList<WishList>,IReadOnlyList<WishListDTO>>(wishlistItem);
            var listPagination = new Pagination<WishListDTO>()
            {
                Data = MappedList,
                PageIndex = paramter.PageIndex,
                PageSize = paramter.PageSize,
                CountOfAllItem = wishlistItem.Count
            };
            return Ok(listPagination);
        }
        [Authorize]
        [HttpDelete("RemoveProductFromWishList/{ProductId}")]
        public async Task<ActionResult<string>> RemoveProductFromWishList(int ProductId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (string.IsNullOrEmpty(user.Id))
                return Unauthorized();
            var result=await _listRepository.RemoveAsync(user.Id, ProductId);
            if (result != true)
                return BadRequest(new ErrorApiResponse(401));
            return Ok("The Product Remove From WishList SuccessFull");
        }
    }
}
