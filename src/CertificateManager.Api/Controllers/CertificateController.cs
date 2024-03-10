using Microsoft.AspNetCore.Mvc;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Application.Abstractions.Interfaces;
using MassTransit;

namespace CertificateManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CertificateController : ControllerBase
{
    private readonly IBus _bus;
    private readonly IPdfCreatorService _documentCreator;

    public CertificateController(IBus bus,IPdfCreatorService documentCreator)
    {
        _bus = bus;
        _documentCreator = documentCreator;
    }

    [HttpPost("create-pdf")]
    public IActionResult CreatePdf(List<UserUpdateDto> users)
    {
        var userUpdateListMessage = new UserUpdateListMessage() { Users = users };

        _bus.Publish(userUpdateListMessage);

        var pdfFile = _documentCreator.CreatePdf(users);

        return File(pdfFile.FileContents, "application/pdf", pdfFile.FileDownloadName);
    }
}