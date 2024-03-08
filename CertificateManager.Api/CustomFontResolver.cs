using PdfSharp.Fonts;

namespace CertificateManager.Api;

public class CustomFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        var fontFilePath = "arial.ttf";

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