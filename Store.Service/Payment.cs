using Microsoft.Extensions.Configuration;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Enum;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.CoreLayer.IUnitOfWork;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using Product = Store.CoreLayer.Entirty.Product;

namespace Store.Service
{
    public class Payment : IPayment
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _cartRepositorty;
        private readonly IUnitOfWork _unitOfWork;

        public Payment(IConfiguration configuration,
            IBasketRepository cartRepositorty, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _cartRepositorty = cartRepositorty;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];
            var basket = await _cartRepositorty.GetBasketAsync(basketId);
            if (basket is null) return null;
            decimal shippingPrice = 0M;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var subTotal = basket.Items.Sum(i => i.Quantity * i.Price);
            var service = new PaymentIntentService();
            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId)) //Create
            {
                var option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" },
                };
                var paymentIntent = await service.CreateAsync(option);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100)
                };
                var paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, option);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await _cartRepositorty.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentStatus(string paymentIntent, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpec(paymentIntent);
            var order = await _unitOfWork.Repository<Order>().GetByIdAsyncWithSpecification(spec);
            if (flag)
                order.Status = OrderStatus.PaymentSuccssed;
            else
                order.Status = OrderStatus.PaymentFailed;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
