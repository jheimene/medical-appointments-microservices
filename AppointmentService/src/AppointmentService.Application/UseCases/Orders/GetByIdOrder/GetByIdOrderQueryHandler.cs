using BuildingBlocks.Application.Common.Errors;
using MediatR;
using OrderService.Application.Abstractions.Interfaces;
using OrderService.Application.Dtos;
using OrderService.Application.Errors;
using OrderService.Application.Mappings;

namespace OrderService.Application.UseCases.Orders.GetByIdOrder
{
    public sealed class GetByIdOrderQueryHandler(IOrderRepository orderRepository) : IRequestHandler<GetByIdOrderQuery, Result<OrderDetailDto>>
    {
        public readonly IOrderRepository _orderRepository = orderRepository;


        public async Task<Result<OrderDetailDto>> Handle(GetByIdOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null) return Result<OrderDetailDto>.Failure(OrderErrors.NotFound(request.OrderId));

            return Result<OrderDetailDto>.Success(order.ToOrderDetailResponse());
        }
    }
}
