using BuildingBlocks.Application.Common.Errors;

namespace OrderService.Application.Errors
{
    public static class OrderErrors
    {
        public static readonly AppError InvalidOrderId =
            new("ORDER.INVALID_ID", "El identificador de la orden no es válido", ErrorType.Validation);

        public static AppError NotFound(Guid orderId) =>
            new("ORDER.NOT_FOUND", $"No se encontró la orden {orderId}", ErrorType.NotFound);

        public static readonly AppError AlreadyPaid =
            new("ORDER.ALREADY_PAID", "La orden ya fue pagada", ErrorType.Conflict);

        public static readonly AppError InvalidStatusForPayment =
            new("ORDER.INVALID_STATUS_FOR_PAYMENT", "La orden no se puede pagar en su estado actual", ErrorType.BusinessRule);

        public static AppError NotFoundCustomer(Guid customerId) =>
            new("CUSTOMER.NOT_FOUND", $"No se encontró el cliente {customerId}", ErrorType.NotFound);
    }
}
