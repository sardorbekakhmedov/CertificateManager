using PdfSharp.Fonts;

namespace CertificateManager.Api.PdfServices;

public class CustomFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        // When you run without Docker
        // var fontFilePath = "PdfServices/arial.ttf";

        // When you work with Docker
        var fontFilePath = "/app/PdfServices/arial.ttf";

        if (File.Exists(fontFilePath))
        {
            return File.ReadAllBytes(fontFilePath);
        }

        throw new FileNotFoundException($"Font file not found: {fontFilePath}");
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (string.Equals(familyName, "Arial", StringComparison.OrdinalIgnoreCase))
        {
            return new FontResolverInfo("Arial");
        }

        return null;
    }
}