using PdfSharp.Fonts;

namespace CertificateManager.Application.Services.PdfServices;

public class CustomSingletonFontResolver : IFontResolver
{
    private static CustomSingletonFontResolver? _instance;

    public static CustomSingletonFontResolver Instance
    {
        get { return _instance ??= new CustomSingletonFontResolver(); }
    }

    private CustomSingletonFontResolver() { }

    public byte[] GetFont(string faceName)
    {
        // C:\Dev\C#\JOB_TASKS\Webase\CertificateManager\CertificateManager.Application\Services\PdfServices\arial.ttf
        // When you run without Docker
        // var fontFilePath = @"CertificateManager.Application/Services/PdfServices/arial.ttf";
        // var fontFilePath = @"C:\Dev\C#\JOB_TASKS\Webase\CertificateManager\CertificateManager.Application\Services\PdfServices\arial.ttf";

        //// When you work with Docker
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