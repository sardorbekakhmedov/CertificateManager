namespace CertificateManager.Domain.Entities;

public class AuditableEntity : BaseEntity
{
    public virtual Guid CreatedById { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual Guid? LastModifiedById { get; set; }
    public virtual DateTime? LastModifiedDate { get; set; }
}