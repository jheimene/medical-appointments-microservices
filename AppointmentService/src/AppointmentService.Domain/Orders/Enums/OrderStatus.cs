namespace OrderService.Domain.Orders.Enums
{
    public enum OrderStatus
    {
        Draft = 1,        
        Processing = 2,
        Confirmed = 3,
        Cancelled = 4,
        Completed = 5,
    }
}