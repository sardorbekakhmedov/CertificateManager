namespace CertificateManager.Domain.Entities;

public class Certificate : AuditableEntity
{
    public byte[] CertificateData { get; set; } = null!;
    public virtual ICollection<User>? Users { get; set; }
}