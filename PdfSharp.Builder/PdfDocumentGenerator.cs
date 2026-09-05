using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Quality;

namespace PdfSharp.Builder;

public class PdfDocumentGenerator
{
    private PdfDocument _document;
    private PdfPage _page;
    private XGraphics _gfx;


    public PdfDocumentGenerator()
    {
        _document = new PdfDocument();
    }

    private double ConvertToPoint(double value)
    {
        return XUnit.FromCentimeter(value).Point;
    }

    public PdfDocumentGenerator AddPage(double height, double width)
    {
        _page = _document.AddPage();
        _gfx = XGraphics.FromPdfPage(_page);
        _page.Height = XUnit.FromCentimeter(height);
        _page.Width = XUnit.FromCentimeter(width);
        return this;
    }

    public PdfDocumentGenerator AddImage(byte[] bytes, double imageHeight, double imageWidth)
    {
        using (var stream = new MemoryStream(bytes))
        {
            XImage image = XImage.FromStream(stream);
            _gfx.DrawImage(image, 0, 0, ConvertToPoint(imageWidth),ConvertToPoint(imageHeight));
        }
        return this;
    }

    public PdfDocumentGenerator AddLine(double spacefromtop,double lineBegin, double lineEnd)
    {
        // Define a pen: color and width
        XPen pen = new XPen(XColors.Black, 1.0);

        // Draw a line from (x1,y1) to (x2,y2) — coordinates in points
        _gfx.DrawLine(pen, lineBegin, spacefromtop, lineEnd, spacefromtop);
        return this;
    }

    public PdfDocumentGenerator AddText(string text, int fontSize, double x, double y)
    {
        // Create a font.
        var font = new XFont("Times New Roman", fontSize, XFontStyleEx.BoldItalic);

        // Draw the text.
        _gfx.DrawString(text, font, XBrushes.Black,new XPoint(ConvertToPoint(x),ConvertToPoint(y)));

        return this;
    }


    public string Generate()
    {
        using (var stream = new MemoryStream())
        {
            _document.Save(stream, false); // false = don't close the stream after saving
            byte[] pdfBytes = stream.ToArray();
            return Convert.ToBase64String(pdfBytes);
        }
    }

    public void SaveAndShow(string savePath)
    {
        // Save the document...
        var filename = PdfFileUtility.GetTempPdfFullFileName(savePath);
        _document.Save(filename);
        // ...and start a viewer.
        PdfFileUtility.ShowDocument(filename);
    }
}
