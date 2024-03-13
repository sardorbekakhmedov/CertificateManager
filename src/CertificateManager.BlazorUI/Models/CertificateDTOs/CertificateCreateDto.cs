namespace CertificateManager.BlazorUI.Models.CertificateDTOs;

public class CertificateCreateDto
{
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}