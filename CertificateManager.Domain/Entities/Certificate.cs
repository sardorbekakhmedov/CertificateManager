namespace CertificateManager.Domain.Entities;

public class Certificate : AuditableEntity
{
    public Guid? UserId { get; set; }
    public  string? Name { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
}