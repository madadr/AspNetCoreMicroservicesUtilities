using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Common.Application.Events
{
    // TODO: Split Mongo document declaration from event... That's awful
    public class TicketBoughtEvent : IIntegrationEvent
    {
        [BsonId] public Guid Id { get; }

        [BsonElement("CustomerId")] public Guid CustomerId { get; }

        [BsonElement("MovieId")] public Guid MovieId { get; }

        [BsonElement("Seat")] public string Seat { get; }

        public TicketBoughtEvent(Guid id, Guid customerId, Guid movieId, string seat)
        {
            Id = id;
            CustomerId = customerId;
            MovieId = movieId;
            Seat = seat;
        }
    }
}