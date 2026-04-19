
namespace ProductService.Domain.Common
{
    public abstract class AggregateRoot<TId, TUser> : AuditableEntity<TId, TUser>
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        protected AggregateRoot() { }

        protected AggregateRoot(TId id) : base(id) { }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

        protected void RemoveDomainEvent(IDomainEvent @event) => _domainEvents.Remove(@event);

        public void ClearDomainEvents() => _domainEvents.Clear();

    }
}
