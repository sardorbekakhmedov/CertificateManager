using CertificateManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;

public interface ICertificateService
{
    Task<Guid> CreateAsync(Certificate entity);
    Task<FileContentResult> GetCertificateById(Guid certificateId);
    Task<(FileContentResult, Guid)> GetLastCreatedCertificate();
}