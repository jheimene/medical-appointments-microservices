namespace OrderService.Domain.Orders
{
    public class OrderTicket //: BaseAuditableEntity
    {
        public Guid ProductId { get; private set; }
        public decimal Price { get; private set; }
        public string Code { get; private set; } = Guid.NewGuid().ToString("N")[..10].ToUpper();  // Value Object


        private OrderTicket()
        {
        }

        public static OrderTicket Create(Guid zoneId, decimal price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor a cero", nameof(price));
            }
            return new OrderTicket
            {
                //Id = ticketId,
                ProductId = zoneId,
                Price = price
            };
        }
    }
}
