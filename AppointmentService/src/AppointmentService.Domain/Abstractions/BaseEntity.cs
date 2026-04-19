
namespace OrderService.Domain.Abstractions
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        protected BaseEntity() { }

        protected BaseEntity(Guid id) => Id = id;

        public override bool Equals(object? obj)
        {
            if (obj is not BaseEntity other) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<Guid>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode() => EqualityComparer<Guid>.Default.GetHashCode(Id!);
    }
}
