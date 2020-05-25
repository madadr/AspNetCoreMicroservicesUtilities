using System;

namespace Common.Application.Events
{
    public class ClientNotPermitted : IEvent
    {
        public Guid OrderId { get; }
        public int ClientId { get; }
        public string Reason { get; }

        public ClientNotPermitted(Guid orderId, int clientId, string reason = "ClientNotPermitted")
        {
            OrderId = orderId;
            ClientId = clientId;
            Reason = reason;
        }
    }
}