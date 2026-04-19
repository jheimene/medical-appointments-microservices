using MassTransit;
using Orders.Contracts.Dtos;

namespace Orders.Saga
{
    public sealed class OrderState : SagaStateMachineInstance
    {
        // Identificador de la saga (OrderId)
        public Guid CorrelationId { get; set; }

        // Estado actual de la saga
        public string CurrentState { get; set; } = default!;

        // Datos de la orden
        public Guid EventId { get; set; }
        public string CustomerId { get; set; } = default!;
        public PaymentRequest Payment { get; set; } = default!;
        public IEnumerable<ZoneRequest> Zones { get; set; } 


        // Auditoria
        public string UserId { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

    }
}
