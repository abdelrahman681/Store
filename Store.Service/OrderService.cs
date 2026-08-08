using Microsoft.AspNetCore.Identity;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IService;
using Store.CoreLayer.IUnitOfWork;
using Store.CoreLayer.Entirty.Enum;

namespace Store.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basket;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPayment _payment;
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;

        public OrderService(IBasketRepository basket,
            IUnitOfWork unitOfWork,IPayment payment,
            INotificationService notificationService,UserManager<AppUser> userManager)
        {
            _basket = basket;
            _unitOfWork = unitOfWork;
            _payment = payment;
            _notificationService = notificationService;
            _userManager = userManager;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, ShippingAddress address)
        {
            var user = await _userManager.FindByEmailAsync(BuyerEmail);
            //1.Get Basket From Basket Repo
            var basket = await _basket.GetBasketAsync(BasketId);
            //2.Get Selected Items at Basket From Product Repo
            var itemOrders = new List<ItemOrder>();
            var Item = new ItemOrder();
            if (basket?.Items.Count()>0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var itemProductOrder = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    Item = new ItemOrder(itemProductOrder, product.Price, item.Quantity);
                    itemOrders.Add(Item);
                }
            }
            //3.Calculate SubTotal
            var SubTotal = itemOrders.Sum(item => item.Quantity * item.Price);
            //4.Get Delivery Method From DeliveryMethod Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            //5.Create Order
            var spec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);
            var previousOrder = await _unitOfWork.Repository<Order>().GetByIdAsyncWithSpecification(spec);
            if (previousOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(previousOrder);
                await _payment.CreateOrUpdatePaymentIntent(BasketId);
            }
            var order = new Order(BuyerEmail, address, deliveryMethod, itemOrders, SubTotal,basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.CompleteAsync();
            await _notificationService.SendNotificationAsync(user.Id,

    "Order Created",
    $"Your order #{order.Id} has been created successfully.");
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(string BuyerEmail, int orderId, string BasketId, int DeliveryMethodId, ShippingAddress address)
        {
            var spec = new OrderWithSpecification(BuyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdAsyncWithSpecification(spec);
            if (order is null)
                return null;

            if (order.Status is not OrderStatus.pending and not OrderStatus.PaymentFailed)
                return null;

            var basket = await _basket.GetBasketAsync(BasketId);
            if (basket is null || basket.Items.Count == 0)
                return null;

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            if (deliveryMethod is null)
                return null;

            var itemOrders = new List<ItemOrder>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (product is null)
                    return null;

                var itemProductOrder = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                itemOrders.Add(new ItemOrder(itemProductOrder, product.Price, item.Quantity));
            }

            order.Address.FName = address.FName;
            order.Address.LName = address.LName;
            order.Address.City = address.City;
            order.Address.Street = address.Street;
            order.Address.Country = address.Country;
            order.DeliveryMethod = deliveryMethod;
            order.SubTotal = itemOrders.Sum(i => i.Quantity * i.Price);

            order.Items.Clear();
            foreach (var item in itemOrders)
                order.Items.Add(item);

            await _payment.CreateOrUpdatePaymentIntent(BasketId);

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();

            var user = await _userManager.FindByEmailAsync(BuyerEmail);
            if (user is not null)
            {
                await _notificationService.SendNotificationAsync(
                    user.Id,
                    "Order Updated",
                    $"Your order #{order.Id} has been updated successfully.");
            }

            return order;
        }

        public async Task<Order?> GetOrderByIdForSpecificUser(string buyerEmail, int OrderId)
        {
            var spec = new OrderWithSpecification(buyerEmail, OrderId);
            var order=await _unitOfWork.Repository<Order>().GetByIdAsyncWithSpecification(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IReadOnlyList<Order?>> GetOrdersForSpecificUser(string Email,BaseParams order)
        {
            var spec = new OrderWithSpecification(Email,order);
            var orders =await  _unitOfWork.Repository<Order>().GetAllAsyncWithSpecification(spec);
            if (orders is null) return null;
            return orders;
        }

    }
}
