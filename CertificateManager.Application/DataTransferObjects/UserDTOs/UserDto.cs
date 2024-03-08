using CertificateManager.Domain.Enums;

namespace Certificate.Application.DataTransferObjects.UserDTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public uint Age { get; set; }
    public string? Email { get; set; }
    public bool HasCertificate { get; set; }
    public EUserRoles UserRoles { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModifiedById { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public virtual CertificateManager.Domain.Entities.Certificate? Certificate { get; set; }
}