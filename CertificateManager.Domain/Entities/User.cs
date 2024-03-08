using CertificateManager.Domain.Enums;

namespace CertificateManager.Domain.Entities;

public class User : AuditableEntity
{
    public required string Username { get; set; }
    public uint Age { get; set; }
    public string? Email { get; set; }
    public bool HasCertificate { get; set; }
    public string PasswordHash { get; set; } = null!;
    public EUserRoles UserRole { get; set; }
    public virtual Certificate? Certificate { get; set; }
}