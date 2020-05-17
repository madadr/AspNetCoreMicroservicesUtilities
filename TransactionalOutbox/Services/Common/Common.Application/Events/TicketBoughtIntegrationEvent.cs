using System;

namespace Common.Application.Events
{
    public class TicketBoughtIntegrationEvent : IIntegrationEvent
    {
        private readonly Guid _commandCustomerId;
        private readonly Guid _commandMovieId;
        private readonly string _commandSeat;

        public TicketBoughtIntegrationEvent(Guid commandCustomerId, Guid commandMovieId, string commandSeat)
        {
            _commandCustomerId = commandCustomerId;
            _commandMovieId = commandMovieId;
            _commandSeat = commandSeat;
        }
    }
}