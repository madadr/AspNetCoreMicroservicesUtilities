using System;

namespace Common.Application.Events
{
    public class TicketBoughtEvent : IIntegrationEvent
    {
        public Guid CustomerId { get; }
        public Guid MovieId { get; }
        public string Seat { get; }

        public TicketBoughtEvent(Guid customerId, Guid movieId, string seat)
        {
            CustomerId = customerId;
            MovieId = movieId;
            Seat = seat;
        }
    }
}