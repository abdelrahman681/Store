using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.IUnitOfWork;
using Store.DTO;
using Store.Errors;
using Stripe;
using System.Security.Claims;
using Product = Store.CoreLayer.Entirty.Product;

namespace Store.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basket;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BasketController(IBasketRepository basket,
            IMapper mapper,IUnitOfWork unitOfWork)
        {
            _basket = basket;
            _mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        [HttpGet("GetBasket/{BasketId}")]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string BasketId)
        {
            var basket = await _basket.GetBasketAsync(BasketId);
            var mappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDTO>(basket);
            return basket is not null ? Ok(mappedBasket) : new CustomerBasket(BasketId);
        }
        [HttpDelete("DeleteBasket")]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            var IsBasketDeleted = await _basket.DeleteBasketAsync(BasketId);
            if (IsBasketDeleted)
                return Ok(true);
            else
                return BadRequest(new ErrorApiResponse(400));
        }
        [HttpPost("UpdateBasket")]
        public async Task<ActionResult<CustomerBasketDTO>> UpdateBasket(CustomerBasketDTO Basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(Basket);
            foreach (var item in mappedBasket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Quantity > product.StockQuantity)
                    return BadRequest($"Only {product.StockQuantity} item(s) available for {product.Name}.");
            }
            var basket = await _basket.UpdateBasketAsync(mappedBasket);
            return basket is not null ? Ok(basket) : NotFound(new ErrorApiResponse(404));
        }
        [Authorize]
        [HttpGet("{orderId}/invoice/download")]
        public async Task<ActionResult> DownloadInvoice(int orderId)
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var pdf = await _basket.GenerateInvoicePdfAsync(email,orderId);

            if (pdf == null)
                return NotFound();

            return File(
                pdf,
                "application/pdf",
                $"Invoice-{orderId}.pdf");
        }
    }
}
