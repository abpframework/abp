using System.Collections.Generic;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.Kernel.Pdf.Navigation;
using iText.StyledXmlParser.Node;

namespace Volo.Docs.Projects.Pdf.IText;

public class HtmlIdTagWorkerFactory : DefaultTagWorkerFactory
{
    private readonly iText.Kernel.Pdf.PdfDocument _pdfDocument;
    private readonly Dictionary<string, int> _pageDestinations = new();

    public HtmlIdTagWorkerFactory(iText.Kernel.Pdf.PdfDocument pdfDocument)
    {
        _pdfDocument = pdfDocument;
    }

    public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
    {
        var tagClass = tag.GetAttribute("class");
        if (tag.Name().Equals("div") && tagClass =="page")
        {
            var id = tag.GetAttribute("id");
            _pageDestinations[id] = _pdfDocument.GetNumberOfPages() + 1;
        }
        
        return base.GetCustomTagWorker(tag, context);
    }

    public void AddNamedDestinations()
    {
        foreach (var pageDestination in _pageDestinations)
        {
            var page = _pdfDocument.GetPage(pageDestination.Value);
            var destination = PdfExplicitDestination.CreateFit(page);
            _pdfDocument.AddNamedDestination(pageDestination.Key, destination.GetPdfObject());
        }
    }
}