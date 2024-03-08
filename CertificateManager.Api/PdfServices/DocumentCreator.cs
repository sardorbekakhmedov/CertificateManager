using Certificate.Application.DataTransferObjects.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace CertificateManager.Api.PdfServices;

public class DocumentCreator
{
    public FileContentResult CreatePdf(UserUpdateDto user)
    {
        GlobalFontSettings.FontResolver = new CustomFontResolver();

        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Arial", 24);
        
        // When you run without Docker
        // XImage image = XImage.FromFile("PdfServices\\backimage.jpg");

        // When you work with Docker
        XImage image = XImage.FromFile("/app/PdfServices/backimage.jpg");

        gfx.DrawImage(image, 0, 0, page.Width, page.Height);

        var text = $"         CERTIFICATE FOR: \n\n" +
                   $"Username: {user.Username}\n" +
                   $"Age: {user.Age}\n" +
                   $"Email: {user.Email}\n" +
                   $"UserRole: {user.UserRole}";

        var tf = new XTextFormatter(gfx);
        var rect = new XRect(50, 50, page.Width + 200, page.Height - 100);
        tf.DrawString(text, font, XBrushes.Red, rect, XStringFormats.TopLeft);

        using var stream = new MemoryStream();
        document.Save(stream, false);
        stream.Position = 0;
        var contentType = "application/pdf";
        var fileName = "UserUpdateInfo.pdf";

        return new FileContentResult(stream.ToArray(), contentType)
        {
            FileDownloadName = fileName
        };
    }
}