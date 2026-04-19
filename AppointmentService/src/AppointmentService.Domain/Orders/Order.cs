using OrderService.Domain.Abstractions;
using OrderService.Domain.Orders.Enums;

namespace OrderService.Domain.Orders
{
    public sealed class Order : AggregateRoot
    {
        public Guid OrderId { get; private set; }
        public string OrderNumber { get; private set; } = default!;
        public Guid CustomerId { get; private set; } = Guid.Empty;
        public string CustomerName { get; private set; } = string.Empty;
        public OrderStatus Status { get; private set; }
        public string Currency { get; private set; } = default!;
        public decimal Subtotal { get; private set; }
        public decimal DiscountTotal { get; private set; }
        public decimal TaxTotal { get; private set; }
        public decimal Total { get; private set; }
        public string? Notes { get; private set; }
        public string? IdempotencyKey { get; private set; }

        private readonly List<OrderItem> _items = [];
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        //public decimal SubTotal => Items.Sum(ot => ot.UnitPrice * ot.Quantity);
        //public decimal Igv => SubTotal * Common.IGV;
        //public decimal Total => SubTotal + Igv;

        private Order() { }

        public static Order Create(
            string orderNumber,
            Guid customerId,
            string customerName,
            string currency,
            string? notes,
            string? idempotency
        //List<OrderItem> details
        )
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("El clientes es obligatorio", nameof(customerId));

            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                OrderId = orderId,
                OrderNumber = orderNumber,
                CustomerId = customerId,
                CustomerName = customerName,
                Currency = currency,
                Notes = notes?.Trim(),
                Status = OrderStatus.Processing,
                IdempotencyKey = idempotency?.Trim()
                //_items = details,
            };

            order.SetCreated("system");

            return order;
        }

        public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity, decimal discountAmount, decimal taxAmount)
        {
            EnsureEditable();

            var item = OrderItem.Create(OrderId, productId, productName, unitPrice, quantity, discountAmount, taxAmount);

            _items.Add(item);

            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            Subtotal = decimal.Round(_items.Sum(x => x.UnitPrice * x.Quantity), 2);
            DiscountTotal = decimal.Round(_items.Sum(x => x.DiscountAmount), 2);
            TaxTotal = decimal.Round(_items.Sum(x => x.TaxAmount), 2);
            Total = decimal.Round(_items.Sum(x => x.LineTotal), 2);
        }

        public void Confirm(string modifiedBy)
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Order must have at least one item.");

            if (Total <= 0)
                throw new InvalidOperationException("Order total must be greater than zero.");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cancelled order cannot be confirmed.");

            Status = OrderStatus.Confirmed;
            //ConfirmedAt = DateTime.UtcNow;
            SetModified(modifiedBy);
        }

        public void Cancel(string modifyBy)
        {
            if (Status == OrderStatus.Cancelled)
                return;
            Status = OrderStatus.Cancelled;
            SetModified(modifyBy);
        }

        private void EnsureEditable()
        {
            if (Status is OrderStatus.Cancelled or OrderStatus.Completed or OrderStatus.Confirmed)
                throw new InvalidOperationException("Order cannot be modified in current state.");
        }

        // Método para recrear la orden a partir de un estado anterior (útil para eventos de reconstrucción)
        public static Order Rehydrate(OrderSnapshot snapshot) {
            var order = new Order
            {
                Id = snapshot.Id,
                OrderId = snapshot.OrderId,
                OrderNumber = snapshot.OrderNumber,
                CustomerId = snapshot.CustomerId,
                CustomerName = snapshot.CustomerName,
                Status = snapshot.Status,
                Currency = snapshot.Currency,
                Subtotal = snapshot.Subtotal,
                DiscountTotal = snapshot.DiscountTotal,
                TaxTotal = snapshot.TaxTotal,
                Total = snapshot.Total,
                Notes = snapshot.Notes,
                IdempotencyKey = snapshot.IdempotencyKey,
                CreatedBy = snapshot.CreatedBy,
                CreatedAt = snapshot.CreateAt,
                LastModifiedBy = snapshot.LastModifiedBy,
                LastModifiedAt = snapshot.LastModifiedAt
            }; 
            
            snapshot.Items.ToList().ForEach(itemSnapshot => {
                var item = OrderItem.Rehydrate(itemSnapshot);
                order._items.Add(item);
            });

            return order;
        }

    }
}
