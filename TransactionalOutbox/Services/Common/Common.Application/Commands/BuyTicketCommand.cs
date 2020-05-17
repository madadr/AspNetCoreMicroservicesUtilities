using System;

namespace Common.Application.Commands
{
    public class BuyTicketCommand : ICommand
    {
        public Guid CustomerId { get; private set; }
        public Guid MovieId { get; private set; }
        public string Seat { get; private set; }

        public BuyTicketCommand()
        {
        }

        public BuyTicketCommand(Guid customerId, Guid movieId, string seat)
        {
            CustomerId = customerId;
            MovieId = movieId;
            Seat = seat;
        }
    }
}