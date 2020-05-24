using System;
using Common.Application.Commands;

namespace Store.Application.Commands
{
    public class BookProduct : ICommand
    {
        public Guid OrderId { get; }
        public int ClientId { get; }
        public int ProductId { get; }
        public double Price { get; }

        public BookProduct(Guid orderId, int clientId, int productId, double price)
        {
            OrderId = orderId;
            ClientId = clientId;
            ProductId = productId;
            Price = price;
        }
    }
}