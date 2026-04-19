using BffService.DTOs;
using BffService.Interfaces;

namespace BffService.Services
{
    public sealed class OrderSummaryComposer : IOrderSummaryComposer
    {
        private readonly IOrderServiceClient _orderClient;
        private readonly ICustomerServiceClient _customerClient;
        private readonly IPaymentServiceClient _paymentClient;
        private readonly IDispatchServiceClient _dispatchClient;
        private readonly ILogger<OrderSummaryComposer> _logger;

        public OrderSummaryComposer(
            IOrderServiceClient orderClient,
            ICustomerServiceClient customerClient,
            IPaymentServiceClient paymentClient,
            IDispatchServiceClient dispatchClient,
            ILogger<OrderSummaryComposer> logger)
        {
            _orderClient = orderClient;
            _customerClient = customerClient;
            _paymentClient = paymentClient;
            _dispatchClient = dispatchClient;
            _logger = logger;
        }

        public async Task<OrderSummaryResponse?> ComposeAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await _orderClient.GetOrderByIdAsync(orderId, cancellationToken);

            if (order is null)
                return null;

            var customerTask = _customerClient.GetCustomerByIdAsync(order.CustomerId, cancellationToken);
            var paymentTask = _paymentClient.GetPaymentByIdAsync(order.PaymentId, cancellationToken);
            var shipmentTask = _dispatchClient.GetShipmentByIdAsync(order.ShipmentId, cancellationToken);

            await Task.WhenAll(customerTask, paymentTask, shipmentTask);

            var customer = await customerTask;
            var payment = await paymentTask;
            var shipment = await shipmentTask;

            if (customer is null || payment is null || shipment is null)
            {
                _logger.LogWarning(
                    "No se pudo componer completamente la orden {OrderId}. Customer: {HasCustomer}, Payment: {HasPayment}, Shipment: {HasShipment}",
                    orderId,
                    customer is not null,
                    payment is not null,
                    shipment is not null);

                return null;
            }

            var items = order.Items
                .Select(x => new OrderItemSummary(
                    x.ProductId,
                    x.ProductName,
                    x.Quantity,
                    x.UnitPrice,
                    x.Quantity * x.UnitPrice))
                .ToList();

            return new OrderSummaryResponse(
                order.Id,
                order.OrderNumber,
                order.CreatedAt,
                new CustomerSummary(
                    customer.Id,
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email),
                new PaymentSummary(
                    payment.Id,
                    payment.Status,
                    payment.Amount,
                    payment.Currency),
                new ShipmentSummary(
                    shipment.Id,
                    shipment.Status,
                    shipment.TrackingCode),
                items,
                order.Total
            );
        }
    }
}
