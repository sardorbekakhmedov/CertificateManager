using Microsoft.AspNetCore.Mvc;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using MassTransit;
using CertificateManager.Application.Abstractions.Interfaces.RepositoryServices;
using Microsoft.AspNetCore.Authorization;
using CertificateManager.Api.SignalRHub;
using Microsoft.AspNetCore.SignalR;

namespace CertificateManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CertificateController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICertificateService _certificateService;
    private readonly IHubContext<CustomHub> _customHubContext;

    public CertificateController(
        IUserService userService, 
        IPublishEndpoint publishEndpoint,
        ICertificateService certificateService,
        IHubContext<CustomHub> customHubContext)
    {
        _certificateService = certificateService;
        _customHubContext = customHubContext;
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

        // Sending an array of bytes via SignalR
        var messageText = "You can download the ready-made certificate that you have created!";
        await _customHubContext.Clients.All.SendAsync("message", messageText);

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

        // Sending an array of bytes via SignalR
        var messageText = "You can download the ready-made certificate that you have created!";
        await _customHubContext.Clients.All.SendAsync("message", messageText);


        return File(file.FileContents, "application/pdf", "Certificate.pdf");
    }
}