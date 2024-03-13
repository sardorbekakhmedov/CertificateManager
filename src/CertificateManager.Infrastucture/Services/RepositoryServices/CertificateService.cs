using AutoMapper;
using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;
using CertificateManager.Application.Exceptions;
using CertificateManager.Application.Services;
using CertificateManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CertificateManager.Infrastructure.Services.RepositoryServices;

public class CertificateService : ICertificateService
{
    private readonly IAppDbContext _dbContext;
    private readonly ICurrentUser _currentUser;
    private readonly IHttpContextHelper _httpContextHelper;

    public CertificateService(
        IAppDbContext dbContext,
        ICurrentUser currentUser,
        IHttpContextHelper httpContextHelper)
    {
        _dbContext = dbContext;
        _httpContextHelper = httpContextHelper;
        _currentUser = currentUser;
    }

    public async Task<Guid> CreateAsync(Certificate entity)
    {
        Guid userid;
        if (_currentUser.UserId is null)
        {
            userid = StaticFields.AdminId;
            // throw new UnauthorizedException("Username is not available. Authentication Failed.");
        }
        else
        {
            userid = (Guid)_currentUser.UserId;
        }

        entity.CreatedById = userid;


        await _dbContext.Certificates.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<FileContentResult> GetCertificateById(Guid certificateId)
    {
        var lastFile = await _dbContext.Certificates.FirstOrDefaultAsync(x => x.Id == certificateId);

        if (lastFile is null)
            throw new NotFoundException("Certificate not found!.");

        var fileResult = new FileContentResult(lastFile.CertificateData, "application/pdf")
        {
            FileDownloadName = "certificate.pdf"
        };

        return fileResult;
    }

    public async Task<(FileContentResult, Guid)> GetLastCreatedCertificate()
    {
        var lastFile = await _dbContext.Certificates
            .OrderByDescending(x => x.CreatedDate)
            .LastOrDefaultAsync();

        if (lastFile is null)
            throw new NotFoundException("Certificate not found!.");

        var fileResult = new FileContentResult(lastFile.CertificateData, "application/pdf")
        {
            FileDownloadName = "certificate.pdf"
        };

        return (fileResult, lastFile.Id);
    }

}