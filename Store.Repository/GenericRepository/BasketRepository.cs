using QuestPDF.Fluent;
using StackExchange.Redis;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.ISepecfication;
using Store.CoreLayer.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Store.Repository.GenericRepository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        private readonly IUnitOfWork _unitOfWork;


        #region Ctor
        public BasketRepository(IConnectionMultiplexer connection,IUnitOfWork unitOfWork)
        {
            _database = connection.GetDatabase();
            this._unitOfWork = unitOfWork;
        } 
        #endregion
        public async Task<bool> DeleteBasketAsync(string basketId)
          =>  await _database.KeyDeleteAsync(basketId);

        public async Task<byte[]> GenerateInvoicePdfAsync(string email,int orderId)
        {
            var spec = new OrderWithSpecification(email, orderId);
            var order = await _unitOfWork.Repository<CoreLayer.Entirty.Order>()
                .GetByIdAsyncWithSpecification(spec);

            if (order == null)
                return null;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Invoice #{order.Id}");
                        col.Item().Text(order.BuyerEmail);

                        foreach (var item in order.Items)
                        {
                            col.Item().Text(
                                $"{item.Product.ProductName} x {item.Quantity} = {item.Price}");
                        }

                        col.Item().Text($"Total : {order.Total}");
                    });
                });
            }).GeneratePdf();

            return pdf;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket=await _database.StringGetAsync(basketId);
            return basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(basket.ToString());
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var Basket = JsonSerializer.Serialize(basket);
            var IsCreateOrUpdated = await _database.StringSetAsync(basket.Id,Basket,TimeSpan.FromDays(3));
            if (IsCreateOrUpdated)
                return await GetBasketAsync(basket.Id);
            else 
                return null;
        }
    }
}
