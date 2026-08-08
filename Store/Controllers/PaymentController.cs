using AutoMapper;
using E_Commerce.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Enum;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.CoreLayer.IUnitOfWork;
using Store.DTO;
using Store.Errors;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace Store.Controllers 
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPayment _paymentService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        const string endpointSecret = "whsec_935224dbc360fb270a794df931e2f5b6f3eea22514673f3d6355f0e9743003cd";
        public PaymentController(IPayment paymentService,
            IMapper mapper,IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this._configuration = configuration;
        }
        [HttpPost("CreateOrUpdatePaymentIntent")]
        [ProducesResponseType(typeof(CustomerBasketDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorApiResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDTO?>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var customerBasket = await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (customerBasket == null)
                return BadRequest(new ErrorApiResponse(400));
            var mappedcustomerCart = _mapper.Map<CustomerBasket, CustomerBasketDTO>(customerBasket);
            return Ok(mappedcustomerCart);
        }
        //https://localhost:7195/Payment/Weebhook
        [HttpPost("Webhook")]
        public async Task<IActionResult> Webhook()
        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);
                switch (stripeEvent.Type)
                {
                    case "payment_intent.succeeded":
                        {
                            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                            var orderId = int.Parse(paymentIntent.Metadata["OrderId"]);

                            var order = await _unitOfWork.Repository<Order>()
                                .GetByIdAsync(orderId);

                            order.Status = OrderStatus.PaymentSuccssed;

                            await _unitOfWork.CompleteAsync();

                            break;
                        }

                    case "payment_intent.payment_failed":
                        {
                            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                            var orderId = int.Parse(paymentIntent.Metadata["OrderId"]);

                            var order = await _unitOfWork.Repository<Order>()
                                .GetByIdAsync(orderId);

                            order.Status = OrderStatus.PaymentFailed;

                            await _unitOfWork.CompleteAsync();

                            break;
                        }
                }

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new ErrorApiResponse(400, ex.Message));
            }
        }

    }
}
