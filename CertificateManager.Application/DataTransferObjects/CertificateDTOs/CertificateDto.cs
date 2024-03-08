namespace Certificate.Application.DataTransferObjects.CertificateDTOs;

public class CertificateDto
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddYears(1);
}