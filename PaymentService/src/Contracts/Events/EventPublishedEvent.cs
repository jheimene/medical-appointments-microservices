
namespace Contracts.Events
{
    public record EventPublishedEvent()
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = default!;
        public DateTime PublicationDate { get; set; }
        public List<ZoneItemPublished> Zones { get; set; } = new();
    }


    public record ZoneItemPublished ()
    {
        public Guid ZoneId { get; set; }
        public string? ZoneName { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
    }

}
