using CertificateManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CertificateManager.Domain.Entities;

public class User : AuditableEntity
{
    public required string Username { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid age.")]
    public int Age { get; set; }
    public string? Email { get; set; }
    public bool HasCertificate { get; set; }
    public Guid? CertificateId { get; set; }
    public string PasswordHash { get; set; } = null!;
    public EUserRoles UserRole { get; set; }

    public virtual Certificate? Certificate { get; set; }
}