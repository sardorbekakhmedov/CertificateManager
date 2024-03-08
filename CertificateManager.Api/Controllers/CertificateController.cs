using Microsoft.AspNetCore.Mvc;
using Certificate.Application.DataTransferObjects.UserDTOs;
using CertificateManager.Api.PdfServices;

namespace CertificateManager.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CertificateController : ControllerBase
{
    private readonly DocumentCreator _documentCreator;

    public CertificateController(DocumentCreator documentCreator)
    {
        _documentCreator = documentCreator;
    }


    [HttpPost("create-pdf")]
    public IActionResult CreatePdf(UserUpdateDto user)
    {
        var pdfFile = _documentCreator.CreatePdf(user);

        return File(pdfFile.FileContents, "application/pdf", pdfFile.FileDownloadName);
    }
}