using System.Reflection;
using PdfSharp.Builder;
using PdfSharp.Fonts;

namespace Pdfsharp.Builder.Tests;

public class PdfDocumentGeneratorTests
{

    [Fact]
    public void PdfWithText()
    {
        // Given
        GlobalFontSettings.UseWindowsFontsUnderWindows = true;
        var ti = new PdfDocumentGenerator();
        var logo = GetEmbeddedResourceImage("Ups_logo.png");
        

        // When
        ti.AddPage(10,10)
        .AddLine(5,0, 300)
        .AddText("Hello from Test", 10,0.5,1)
        .AddLine(20, 0, 300)
        .AddPage(10,10)
        .AddText("Page 2", 10, 0.5, 1)
        .AddImage(logo, 4,4);

        // Then
        ti.SaveAndShow(@"C:\temp\");
        Assert.True(true);
    }

    private byte[] GetEmbeddedResourceImage(string name)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        string? resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(name, StringComparison.OrdinalIgnoreCase));

        if (resourceName == null)
            throw new FileNotFoundException("Embedded resource not found.");

        byte[] imageBytes;
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        using (MemoryStream ms = new MemoryStream())
        {
            stream.CopyTo(ms);
            imageBytes = ms.ToArray();
        }

        return imageBytes;
    }
}
