using CertificateManager.Domain.Enums;

namespace Certificate.Application.SortFilters.FilterEntities;

public class CertificateFiler
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddYears(1);

}