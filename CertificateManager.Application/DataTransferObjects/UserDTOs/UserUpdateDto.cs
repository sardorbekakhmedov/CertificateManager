using CertificateManager.Domain.Enums;

namespace Certificate.Application.DataTransferObjects.UserDTOs;

public class UserUpdateDto
{
    public string? Username { get; set; }
    public uint? Age { get; set; }
    public string? Email { get; set; }
    public EUserRoles? UserRole { get; set; }
}