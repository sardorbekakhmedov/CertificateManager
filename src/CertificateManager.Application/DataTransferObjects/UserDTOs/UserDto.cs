using CertificateManager.Application.DataTransferObjects.CertificateDTOs;
using CertificateManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CertificateManager.Application.DataTransferObjects.UserDTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;

    [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid age.")]
    public int Age { get; set; }
    public string? Email { get; set; }
    public bool HasCertificate { get; set; }
    public EUserRoles UserRoles { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedById { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public virtual CertificateDto? Certificate { get; set; }
}