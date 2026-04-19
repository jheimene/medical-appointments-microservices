namespace OrderService.Domain.Orders
{
    public sealed class OrderItem
    {
        public Guid OrderItemId { get; private set; } = Guid.NewGuid();
        public Guid OrderId { get; private set; }

        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; } = default!;
        public decimal UnitPrice { get; private set; } 
        public int Quantity { get; private set; }

        public decimal DiscountAmount { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal LineTotal { get; private set; }

        private OrderItem()  {}

        public static OrderItem Create(
            Guid orderId,
            Guid productId,
            string productName,
            decimal unitPrice, 
            int quantity,
            decimal discountAmount,
            decimal taxAmount
        )
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("La Orden es requeridad.", nameof(orderId)); 

            if (unitPrice <= 0)   
                throw new ArgumentException("El precio unitario debe ser mayor a cero", nameof(unitPrice));
            
            if (quantity <= 0)         
                throw new ArgumentException("La cantidad debe ser mayor a cero", nameof(quantity));
            

            var orderItem =  new OrderItem
            {
                OrderItemId = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = productId,
                ProductName = productName,
                UnitPrice = unitPrice,
                Quantity = quantity,
                DiscountAmount = decimal.Round(discountAmount, 2),
                TaxAmount = decimal.Round(taxAmount, 2)
            };
            orderItem.Recalculate();

            return orderItem;
        }

        private void Recalculate()
        {
            var subtotal = UnitPrice * Quantity;
            var total = subtotal - DiscountAmount + TaxAmount;

            if (total < 0)
                throw new InvalidOperationException("LineTotal no puede ser negativo");

            LineTotal = decimal.Round(total, 2, MidpointRounding.AwayFromZero);
        }

        public static OrderItem Rehydrate(OrderItemSnapshot snapshot)
        {
            var orderItem = new OrderItem
            {
                OrderItemId = snapshot.Id,
                OrderId = snapshot.OrderId,
                ProductId = snapshot.ProductId,
                ProductName = snapshot.ProductName,
                UnitPrice = snapshot.UnitPrice,
                Quantity = snapshot.Quantity,
                DiscountAmount = snapshot.DiscountAmount,
                TaxAmount = snapshot.TaxAmount
            };
            orderItem.Recalculate();
            return orderItem;
        }
    }
}
