
using OrderService.Domain.Orders;

namespace OrderService.Infrastructure.Persistence.Mongo
{
    public static class OrderDocumentMapper
    {
        public static OrderDocument ToDocument(this Order order)
        {
            return new OrderDocument
            {
                Id = order.Id,
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                Status = order.Status,
                Currency = order.Currency,
                Subtotal = order.Subtotal,
                DiscountTotal = order.DiscountTotal,
                TaxTotal = order.TaxTotal,
                Total = order.Total,
                Notes = order.Notes,
                IdempotencyKey = order.IdempotencyKey,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                Items = [.. order.Items.Select(i => new OrderItemDocument
                {
                    Id = i.OrderItemId,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    DiscountAmount= i.DiscountAmount,
                    TaxAmount = i.TaxAmount,
                    LineTotal = i.LineTotal
                })]
            };
        }

        public static Order ToDomain(this OrderDocument document)
        {
            var order = Order.Rehydrate(new OrderSnapshot(
                Id: document.Id,
                OrderId: document.OrderId,
                OrderNumber: document.OrderNumber,
                CustomerId: document.CustomerId,
                CustomerName: document.CustomerName,
                Status: document.Status,
                Currency: document.Currency,
                Subtotal: document.Subtotal,
                DiscountTotal: document.DiscountTotal,
                TaxTotal: document.TaxTotal,
                Total: document.Total,
                Notes: document.Notes,
                IdempotencyKey: document.IdempotencyKey,
                Items: [.. document.Items.Select(i => new OrderItemSnapshot(
                    Id: i.Id,
                    OrderId: i.OrderId,
                    ProductId: i.ProductId,
                    ProductName: i.ProductName,
                    UnitPrice: i.UnitPrice,
                    Quantity: i.Quantity,
                    DiscountAmount: i.DiscountAmount,
                    TaxAmount: i.TaxAmount,
                    LineTotal: i.LineTotal
                ))],
                CreateAt: document.CreatedAt,
                CreatedBy: document.CreatedBy,
                LastModifiedAt: document.LastModifiedAt,
                LastModifiedBy: document.LastModifiedBy
            ));

            return order;
        }
    }

}
