using System;

namespace Orders.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public OrderState State { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime? PlacedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public Order(Guid id, int clientId, int productId, double price, OrderState state = OrderState.Created,
            DateTime? placedAt = null, DateTime? approvedAt = null, DateTime? cancelledAt = null,
            string additionalInfo = "")
        {
            Id = id;
            ClientId = clientId;
            ProductId = productId;
            Price = price;
            State = state;
            AdditionalInfo = additionalInfo;
            PlacedAt = placedAt;
            ApprovedAt = approvedAt;
            CancelledAt = cancelledAt;
        }
    }
}