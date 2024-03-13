using Microsoft.AspNetCore.Mvc;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using MassTransit;
using CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using CertificateManager.Application.Abstractions.Interfaces;

namespace CertificateManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CertificateController : ControllerBase
{
    private readonly IPdfCreatorService _pdfCreatorService;
    private readonly IAppDbContext _dbContext;
    private readonly ICertificateService _certificateService;
    private readonly IUserService _userService;
    private readonly IPublishEndpoint _publishEndpoint;

    public CertificateController(IPdfCreatorService pdfCreatorService, IAppDbContext dbContext, ICertificateService certificateService, IUserService userService, IPublishEndpoint publishEndpoint)
    {
        _pdfCreatorService = pdfCreatorService;
        _dbContext = dbContext;
        _certificateService = certificateService;
        _userService = userService;
        this._publishEndpoint = publishEndpoint;
    }

    [HttpPost("download-pdf")]
    public async Task<IActionResult> CreatePdfWithResponseCertificateId(List<UserUpdateDto> users)
    {
        var userUpdateListMessage = new UserUpdateListMessage() { Users = users };

        await _publishEndpoint.Publish(userUpdateListMessage);

        // FileContentResult, Guid
        var (file, certificateId) = await _certificateService.GetLastCreatedCertificate();

        await _userService.UpdateUserCertificateAsync(certificateId, users);

        return Ok(certificateId);
    }

    [HttpGet("{certificateId:guid}")]
    public async Task<IActionResult> GetCertificateById(Guid certificateId)
    {
        FileContentResult file = await _certificateService.GetCertificateById(certificateId);

        return File(file.FileContents, "application/pdf", "Certificate.pdf");
    }


    [HttpPost("create-pdf")]
    public async Task<IActionResult> CreatePdf(List<UserUpdateDto> users)
    {
        var userUpdateListMessage = new UserUpdateListMessage() { Users = users };

/*        FileContentResult pdfFile = _pdfCreatorService.CreatePdf(users);

        // Getting the contents of a file as an array of bytes
        byte[] fileBytes = pdfFile.FileContents;

        var certificate = new Certificate()
        {
            CertificateData = fileBytes
        };

        await _certificateService.CreateAsync(certificate);*/

        await _publishEndpoint.Publish(userUpdateListMessage);

        // FileContentResult, Guid
        var (file, certificateId) = await _certificateService.GetLastCreatedCertificate();

        await _userService.UpdateUserCertificateAsync(certificateId, users);

        return File(file.FileContents, "application/pdf", "Certificate.pdf");
    }
}