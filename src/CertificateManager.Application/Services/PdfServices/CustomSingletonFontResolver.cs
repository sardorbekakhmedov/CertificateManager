using PdfSharp.Fonts;
using System.Reflection;

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
        // When you run without Docker
        var projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (projectPath is null)
            throw new FileNotFoundException($"Font file not found: {projectPath} arial.ttf");

        var fontFilePath = Path.Combine(projectPath, "arial.ttf");

        //// When you work with Docker
        // var fontFilePath = "/app/PdfServices/arial.ttf";

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