namespace OrderService.Domain.Abstractions
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        protected BaseAuditableEntity() { }

        protected BaseAuditableEntity(Guid id) : base(id) { }


        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }


        protected void SetCreated(string user)
        {
            CreatedBy = user;
            CreatedAt = DateTime.Now;
        }

        protected void SetCreated(string user, DateTime now)
        {
            CreatedBy = user;
            CreatedAt = now;
        }

        protected void SetModified(string user)
        {
            LastModifiedBy = user;
            LastModifiedAt = DateTime.Now;
        }
        protected void SetModified(string user, DateTime now)
        {
            LastModifiedBy = user;
            LastModifiedAt = now;
        }

        protected void SetDeleted(string user)
        {
            IsDeleted = true;
            DeletedBy = user;
            DeletedAt = DateTime.Now;
        }
    }
}
