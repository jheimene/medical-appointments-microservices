using BuildingBlocks.Application.Common.Errors;
using MediatR;
using OrderService.Application.Abstractions;
using OrderService.Application.Abstractions.Interfaces;
using OrderService.Application.Abstractions.Services;
using OrderService.Application.Dtos;
using OrderService.Application.Errors;
using OrderService.Application.Mappings;
using OrderService.Domain.Orders;

namespace OrderService.Application.UseCases.Orders.CreateOrder
{
    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<CreateOrderCommandResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductReadService _productReadService;
        private readonly ICustomerReadService _customerReadService;
        private readonly IPaymentProcessService _paymentProcessService;
        private readonly IOrderNumberGenerator _orderNumberGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            ICustomerReadService customerReadService,
            IProductReadService productReadService,
            IPaymentProcessService paymentProcessService,
            IOrderNumberGenerator orderNumberGenerator,
            IUnitOfWork unitOfWork
        )
        {
            _orderRepository = orderRepository;
            _customerReadService = customerReadService;
            _productReadService = productReadService;
            _paymentProcessService = paymentProcessService;
            _orderNumberGenerator = orderNumberGenerator;

            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateOrderCommandResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //if (!string.IsNullOrWhiteSpace(request.IdempotencyKey))
            //{
            //    var exists = await _orderRepository.ExistsByIdEmpotencyKeyAsync(request.IdempotencyKey, cancellationToken);
            //    if (exists) { throw new InvalidOperationException("Ya existe una orden con el mismo idempotencyKey."); }
            //}

            var orderExisting = await _orderRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey!, cancellationToken);
            if (orderExisting is not null) return Result<CreateOrderCommandResponse>.Success(orderExisting.ToCreateOrderReponse());

            var customer = await _customerReadService.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null || !customer.IsActive)
            {
                //throw new InvalidOperationException("El cliente no existo o esta inactivo");
                return Result<CreateOrderCommandResponse>.Failure(OrderErrors.NotFoundCustomer(request.CustomerId));
            }

            var orderNumber = await _orderNumberGenerator.GenerateAsync(cancellationToken);

            var order = Order.Create(
                orderNumber,
                request.CustomerId, 
                $"{customer.Name} {customer.LastName}",
                request.Currency,  // Validar la moneda
                request.Notes,
                request.IdempotencyKey
            );

            foreach (var item in request.Items)
            {
                var product = await _productReadService.GetByIdAsync(item.ProductId, cancellationToken);
                if (product is null || !product.IsActive)
                    throw new InvalidOperationException($"El producto {item.ProductId} no existe o esta inactivo");

                // Validación de Stock

                // Validación de la moneda

                // Reglas de descuento y Impuesto
                decimal discountAmount = 0m;
                decimal taxAmount = 0m;

                order.AddItem(
                    product.ProductId,
                    product.Name,
                    product.Price,
                    item.Quantity,
                    discountAmount,
                    taxAmount
                );
            }

            await _orderRepository.CreateAsync(order, cancellationToken);
            await _orderRepository.SaveChangesAsync(cancellationToken);
            //await _unitOfWork.SaveChangesAsync();

            //if (!string.IsNullOrWhiteSpace(request.Provider))
            //{
            //    // Integración con la pasarela de pago
            //    var paymentResult = await _paymentProcessService.ProcessPaymentAsync(new ProcessPaymentRequest(
            //        request.Provider ?? "",
            //        request.Currency,
            //        order.Total,
            //        order.Id,
            //        orderNumber,
            //        request.CustomerId,
            //        $"{customer.Name} {customer.LastName}",
            //        "system",
            //        ""
            //    ), cancellationToken);

            //    if (paymentResult is not null && paymentResult.IsSuccess)
            //        order.Confirm(request.CreatedBy);
            //    else
            //        order.Cancel(request.CreatedBy);

            //    await _orderRepository.SaveChangesAsync(cancellationToken);
            //}

            return Result<CreateOrderCommandResponse>.Success(order.ToCreateOrderReponse());
        }
    }
}
