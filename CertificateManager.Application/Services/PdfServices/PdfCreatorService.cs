using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using CertificateManager.Application.Abstractions.Interfaces;
using CertificateManager.Application.DataTransferObjects.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Fonts;
using PdfSharp.Pdf;

namespace CertificateManager.Application.Services.PdfServices;

public class PdfCreatorService : IPdfCreatorService
{
    public PdfCreatorService()
    {
        GlobalFontSettings.FontResolver = CustomSingletonFontResolver.Instance;
    }

    public FileContentResult CreatePdf(List<UserUpdateDto> users)
    {
        var document = new PdfDocument();
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Arial", 23);

        ////  When you run without Docker
        // XImage image = XImage.FromFile(@"C:\Dev\C#\JOB_TASKS\Webase\CertificateManager\CertificateManager.Application\Services\PdfServices\backimage.jpg");
        //  XImage image = XImage.FromFile("CertificateManager.Application/Services/PdfServices/backimage.jpg");

        ////  When you work with Docker
        XImage image = XImage.FromFile("/app/PdfServices/backimage.jpg");

        gfx.DrawImage(image, 0, 0, page.Width, page.Height);

        var text = "            CERTIFICATE FOR: \n\n";

        foreach (var user in users)
        {
            text += $"Username: {user.Username}, " +
                    $"Age: {user.Age}, " +
                    $"UserRole: {user.UserRole}\n";
        }

        var tf = new XTextFormatter(gfx);
        var rect = new XRect(50, 50, page.Width + 200, page.Height - 100);
        tf.DrawString(text, font, XBrushes.Red, rect, XStringFormats.TopLeft);

        using var stream = new MemoryStream();
        document.Save(stream, false);
        stream.Position = 0;
        var contentType = "application/pdf";
        var fileName = "UserCertificate.pdf";

        return new FileContentResult(stream.ToArray(), contentType)
        {
            FileDownloadName = fileName
        };
    }
}