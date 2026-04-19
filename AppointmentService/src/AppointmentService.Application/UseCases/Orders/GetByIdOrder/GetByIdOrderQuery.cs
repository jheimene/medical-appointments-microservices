using BuildingBlocks.Application.Common.Errors;
using MediatR;
using OrderService.Application.Dtos;

namespace OrderService.Application.UseCases.Orders.GetByIdOrder
{
    public sealed record GetByIdOrderQuery(Guid OrderId) : IRequest<Result<OrderDetailDto>>;
}
