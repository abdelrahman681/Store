using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.DTO;
using Store.Errors;
using Store.Helpers;
using System.Security.Claims;

namespace Store.Controllers
{

    public class ReviewController : BaseApiController
    {
        private readonly IReviewService _review;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService review,UserManager<AppUser> userManager,IMapper mapper)
        {
            this._review = review;
            this._userManager = userManager;
            this._mapper = mapper;
        }
        [Authorize]
        [HttpPost("AddReview")]
        public async Task<ActionResult<ReviewToReturnDTO>> AddReview(AddReviewDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _review.AddReviewAsync(dto.ProductId,dto.Rating,email,dto.Comment);
            if (result is null) return BadRequest(new ErrorApiResponse(400, "You have already reviewed this product."));
            var mappedReviewToReturnDTO = _mapper.Map<Review, ReviewToReturnDTO>(result);
            return Ok(mappedReviewToReturnDTO);
        }
        [Authorize]
        [HttpPut("UpdateReview")]
        public async Task<ActionResult<string>> UpdateReview(UpdateReviewDTO dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _review.UpdateReviewAsync(dto.Id,dto.Rating,email,dto.Comment);
            if (result is false) return BadRequest(new ErrorApiResponse(400, "You Can Not Updated This Review")); 
            return Ok("The Review Updated SuccessFull");
        }
        [Authorize]
        [HttpDelete("DeleteReview/{Id}")]
        public async Task<ActionResult<string>> DeleteReview(int Id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result =await _review.DeleteReviewAsync(Id,email);
            if (result is false) return BadRequest(new ErrorApiResponse(400,"You Can Not Delete This Review"));
            return Ok("The Review Deleted SuccessFull");
        }
        [Cache(30)]
        [HttpGet("GetReviewForProduct")]
        public async Task<ActionResult<Pagination<IReadOnlyList<ReviewToReturnDTO>>>> GetReviewForProduct([FromQuery]ReviewParameter parameter)
        {
            var result = await _review.GetProductReviewsAsync(parameter);
            var mappedReview = _mapper.Map<IReadOnlyList<Review>, IReadOnlyList<ReviewToReturnDTO>>(result);
            var PaginationReview = new Pagination<ReviewToReturnDTO>()
            {
                CountOfAllItem= mappedReview.Count,
                Data = mappedReview,
                PageIndex=parameter.PageIndex,
                PageSize=parameter.PageSize,
            };
            return Ok(PaginationReview);
        }
    }
}
