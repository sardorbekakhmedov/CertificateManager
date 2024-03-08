using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Certificate.Application.Services.PdfServices;

public class PdfCreator
{
    public IActionResult GenerateCertificate(string firstName, string lastName, string email, string organization)
    {
        GlobalFontSettings.FontResolver = new CustomFontResolver();

        using (var rsa = RSA.Create(2048))
        {
            var distinguishedName = $"cn={firstName} {lastName}, ou={organization}, o={organization}, c=US, email={email}";
            var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
            var certificate = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));

            // Create PDF with certificate information
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 25);

            // Creating a string with information about the certificate
            var certificateInfo = $"Certificate for: {firstName} {lastName}\n\n";
            certificateInfo += $"Email: {email}\n";
            certificateInfo += $"Organization: {organization}\n";
            certificateInfo += $"Valid from: {certificate.NotBefore}\n";
            certificateInfo += $"Valid until: {certificate.NotAfter}\n";
            certificateInfo += $"Thumbprint: {certificate.Thumbprint}\n";

            var tf = new XTextFormatter(gfx);
            var rect = new XRect(10, 10, page.Width - 20, page.Height - 20);
            // Draw text with automatic line wrapping
            tf.DrawString(certificateInfo, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            // Save PDF to memory and return as file
            using (var stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/pdf")
                {
                    FileDownloadName = "certificate.pdf"
                };
            }
        }
    }
}