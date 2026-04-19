using MongoDB.Bson.Serialization.Attributes;
using OrderService.Domain.Orders.Enums;

namespace OrderService.Infrastructure.Persistence.Mongo
{
    public sealed class OrderDocument
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; }
        [BsonElement("internalId")]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; set; }
        [BsonRequired]
        public string OrderNumber { get; set; } = default!;
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid CustomerId { get; set; }
        [BsonRequired]
        [BsonElement("customerName")]
        public string CustomerName { get; set; } = default!;
        [BsonRequired]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public OrderStatus Status { get; set; }
        public string Currency { get; set; } = default!;
        public decimal Subtotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal Total { get; set; }
        public string? Notes { get; set; }
        [BsonRequired]
        public string? IdempotencyKey { get; set; }

        public List<OrderItemDocument> Items { get; set; } = [];


        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedAt { get; set; }
    }

    public sealed class OrderItemDocument
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid Id { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal LineTotal { get; set; }
    }
}
