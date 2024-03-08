using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Fonts;
using PdfSharp.Drawing.Layout;
using Certificate.Application.DataTransferObjects.UserDTOs;

namespace CertificateManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CertificateController : ControllerBase
{
    
    [HttpPost("pdf")]
    public IActionResult CreatePdf(UserUpdateDto user)
    {
        GlobalFontSettings.FontResolver = new CustomFontResolver();

        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Arial", 18);

        var text = $"               Certificate for:  \n\n" +
                   $"Username: {user.Username},    " +
                   $"Age: {user.Age},   " +
                   $"Email: {user.Email},   " +
                   $"UserRole: {user.UserRole}";

        var tf = new XTextFormatter(gfx);
        tf.DrawString(text, font, XBrushes.Black, new XRect(10, 10, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

        var stream = new MemoryStream();
        document.Save(stream, false);
        stream.Position = 0;

        return File(stream, "application/pdf", "UserUpdateInfo.pdf");
    }
}