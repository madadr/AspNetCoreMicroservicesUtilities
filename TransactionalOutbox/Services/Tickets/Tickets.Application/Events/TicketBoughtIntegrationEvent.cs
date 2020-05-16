using System;
using Tickets.Application.Commands.Handlers;

namespace Tickets.Application.Events
{
    public class TicketBoughtIntegrationEvent : IIntegrationEvent
    {
        public TicketBoughtIntegrationEvent(Guid commandCustomerId, string commandSeat)
        {
        }
    }
}