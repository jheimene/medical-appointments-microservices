using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Orders;

namespace OrderService.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToCollection("Orders");

            builder.HasKey(x => x.OrderId);

            // OrderId PK
            builder.Property(x => x.OrderId)
                .HasBsonRepresentation(BsonType.String)
                .HasElementName("_id")
                .IsRequired();

            builder.Property(x => ((AggregateRoot)x).Id) // Si Id viene de la base
                .HasElementName("internalId");

            builder.Property(x => x.OrderNumber)
                .HasBsonRepresentation(BsonType.String).IsRequired();

            builder.Property(x => x.CustomerId)
                .HasBsonRepresentation(BsonType.String).IsRequired();

            builder.Property(x => x.CustomerName)
                .HasBsonRepresentation (BsonType.String).IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>().IsRequired();

            builder.Property(x => x.IdempotencyKey)
                .IsRequired();

            builder.OwnsMany(o => o.Items, od =>
            {
                od.HasKey(x => x.OrderItemId);
                od.Property(x => x.OrderItemId)
                    .HasElementName("_id")
                    .HasBsonRepresentation(BsonType.String)
                    .IsRequired();

                od.HasElementName("Items");
                //od.Property(x => x.OrderId).HasBsonRepresentation(BsonType.String).IsRequired();
                od.Property(d => d.ProductId).HasBsonRepresentation(BsonType.String).IsRequired();
                od.Property(d => d.UnitPrice).HasBsonRepresentation(BsonType.Decimal128);
                od.Property(d => d.Quantity).HasBsonRepresentation(BsonType.Int32);
            });

            builder.Ignore(builder => builder.DomainEvents);               

            builder.HasIndex(x => x.CustomerId);
        }
    }
}
