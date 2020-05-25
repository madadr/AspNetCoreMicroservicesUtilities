using System;

namespace Common.Application.Commands
{
    public class ChargePayment : ICommand
    {
        public Guid OrderId { get; }
        public int ClientId { get; }
        public int ProductId { get; }
        public double Price { get; }

        public ChargePayment(Guid orderId, int clientId, int productId, double price)
        {
            OrderId = orderId;
            ClientId = clientId;
            ProductId = productId;
            Price = price;
        }
    }
}