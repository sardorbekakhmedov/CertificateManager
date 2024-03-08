using CertificateManager.Domain.Enums;

namespace Certificate.Application.SortFilters.FilterEntities;

public class UserFilter : PaginationParams
{
    public string? Username { get; set; }
    public uint? Age { get; set; }
    public bool? HasCertificate { get; set; }
    public EUserRoles? UserRole { get; set; }

    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
}