using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Enum;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.CoreLayer.IUnitOfWork;
using Store.DTO;
using Store.Errors;
using Store.Helpers;
using System.Security.Claims;

namespace Store.Controllers
{

    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IOrderService orderService,
            UserManager<AppUser> userManager,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _userManager = userManager;
            _mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO order)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddres = _mapper.Map<ShippingAddressDTO, ShippingAddress>(order.Address);
            var Order= await _orderService.CreateOrderAsync(email, order.basketId,order.DeliveryMethodId, MappedAddres);
            if (order is null) return BadRequest(new ErrorApiResponse(400));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDTO>(Order);
            return Ok(MappedOrder);
        }
        [HttpPut("{orderId}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> UpdateOrder(UpdateOrderDTO order,int orderId)  
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var address = _mapper.Map<ShippingAddressDTO, ShippingAddress>(order.ShippingAddress);

            var updatedOrder = await _orderService.UpdateOrderAsync(email, orderId, order.basketId, order.DeliveryMethodId, address);
            if (updatedOrder is null)
                return BadRequest(new ErrorApiResponse(400, "Unable to update order. It may not exist, belong to you, or is no longer editable."));
            var mappedOrder = _mapper.Map<Order, OrderToReturnDTO>(updatedOrder);
            return Ok(mappedOrder);
        }
        [HttpGet("GetOrderByIdForSpecificUser/{OrderId}")]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForSpecificUser(int OrderId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForSpecificUser(email, OrderId);
            if (Order is null) return BadRequest(new ErrorApiResponse(400));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDTO>(Order);
            return Ok(MappedOrder);
        }
        [HttpGet("GetOrdersForSpecificUser")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<Pagination<OrderToReturnDTO>>>> GetOrdersForSpecificUser([FromQuery]BaseParams order)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrdersForSpecificUser(email,order);
            if (Order is null) return BadRequest(new ErrorApiResponse(400));
            var MappedOrder = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(Order);
            var orderPagination = new Pagination<OrderToReturnDTO>()
            {
                PageIndex = order.PageIndex,
                PageSize= order.PageSize,
                Data = MappedOrder,
                CountOfAllItem = MappedOrder.Count
            };
            return Ok(orderPagination);
        }
        [HttpPost("CancelOrder/{orderId}")]
        [Authorize]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var spec = new OrderWithSpecification(email, orderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdAsyncWithSpecification(spec);
            if (order == null)
                return NotFound("Order not found");

            if (order.Status == OrderStatus.Cancelled)
                return BadRequest("Order already cancelled");

            if (order.Status == OrderStatus.Delivered)
                return BadRequest("Delivered orders cannot be cancelled");

            order.Status = OrderStatus.Cancelled;

            _unitOfWork.Repository<Order>().Update(order);

            var result = await _unitOfWork.CompleteAsync();

            if (result <= 0)
                return BadRequest("Failed to cancel order");

            return Ok("Order cancelled successfully");
        }
    }
}
