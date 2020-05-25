using System;

namespace Store.Core.Entities
{
    public class Reservation
    {
        public Guid OrderId { get; }
        public int ClientId { get; }
        public int ProductId { get; }

        public Reservation(Guid orderId, int clientId, int productId)
        {
            OrderId = orderId;
            ClientId = clientId;
            ProductId = productId;
        }
    }
}