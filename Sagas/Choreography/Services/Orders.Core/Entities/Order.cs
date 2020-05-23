using System;

namespace Orders.Core.Entities
{
    public class Order
    {
        public Guid OrderId { get; }
        public int CustomerId { get; }
        public int ProductId { get; }
        public double Price { get; }
        public OrderState State { get; }
        public string AdditionalInfo { get; }

        public Order(Guid orderId, int customerId, int productId, double price, OrderState state = OrderState.Created, string additionalInfo = "")
        {
            OrderId = orderId;
            CustomerId = customerId;
            ProductId = productId;
            Price = price;
            State = state;
            AdditionalInfo = additionalInfo;
        }
    }
}