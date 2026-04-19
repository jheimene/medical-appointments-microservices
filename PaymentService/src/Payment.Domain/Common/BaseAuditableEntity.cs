
namespace PaymentService.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime ModifiedDate { get; set; }
    public string ModifiedBy { get; set; } = default!;
}

