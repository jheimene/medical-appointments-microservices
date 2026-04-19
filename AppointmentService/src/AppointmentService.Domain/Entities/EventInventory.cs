

using Events.Domain.Abstractions;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Domain.Entities
{
    public class EventInventory : BaseAuditableEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; private set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid EventId { get; private set; }
        public string EventName { get; private set; } = string.Empty;

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid ZoneId { get; private set; }
        public string ZoneName { get; private set; } = string.Empty;
        public int Capacity { get; private set; }
        public int Sold { get; private set; }
        public int Reserved { get; private set; }
        public int Available => Capacity - (Sold + Reserved);

        private EventInventory() { }

        public static EventInventory Create(
            Guid eventId,
            string eventName,
            Guid zoneId,
            string zoneName,
            int capacity
        ) => new EventInventory
            {
                EventId = eventId,
                EventName = eventName,
                ZoneId = zoneId,
                ZoneName = zoneName,
                Capacity = capacity,
                Sold = 0,
                Reserved = 0
            };
    }
}
