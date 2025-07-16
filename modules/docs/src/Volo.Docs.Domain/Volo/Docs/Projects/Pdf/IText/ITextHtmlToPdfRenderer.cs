using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Utils;

namespace Volo.Docs.Projects.Pdf.IText;

public class ITextHtmlToPdfRenderer : IHtmlToPdfRenderer, ITransientDependency
{
    protected IOptions<DocsProjectPdfGeneratorOptions> Options { get; }
    
    public ITextHtmlToPdfRenderer(IOptions<DocsProjectPdfGeneratorOptions> options)
    {
        Options = options;
    }
    
    public virtual Task<Stream> RenderAsync(string title, string html, List<PdfDocument> documents)
    {
        var pdfStream = new MemoryStream();
        using (var pdfWriter = new PdfWriter(pdfStream))
        {
            pdfWriter.SetCloseStream(false);
            using (var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfWriter))
            {
                pdfDocument.GetDocumentInfo().SetTitle(title);
                CreatePdfFromHtml(html, pdfDocument);
                AddOutlinesToPdf(pdfDocument, documents);
            }
        }
            
        pdfStream.Position = 0;
        return Task.FromResult<Stream>(pdfStream);
    }

    private void CreatePdfFromHtml(string html, iText.Kernel.Pdf.PdfDocument pdfDocument)
    {
        var converter = new ConverterProperties();
        var tagWorkerFactory = new HtmlIdTagWorkerFactory(pdfDocument);
        converter.SetTagWorkerFactory(tagWorkerFactory);
        
        HtmlConverter.ConvertToDocument(html, pdfDocument, converter);
        
        tagWorkerFactory.AddNamedDestinations();
    }

    private void AddOutlinesToPdf(iText.Kernel.Pdf.PdfDocument pdfDocument, List<PdfDocument> documents)
    {
        var pdfOutlines = pdfDocument.GetOutlines(false);
        BuildPdfOutlines(pdfOutlines, documents);
    }

    private void BuildPdfOutlines(PdfOutline parentOutline, List<PdfDocument> pdfDocumentNodes)
    {
        foreach (var pdfDocumentNode in pdfDocumentNodes)
        {
            if (pdfDocumentNode.IgnoreOnOutline)
            {
                continue;
            }
            
            var outline = parentOutline.AddOutline(pdfDocumentNode.Title);
            if (!pdfDocumentNode.Id.IsNullOrWhiteSpace())
            {
                outline.AddAction(UrlHelper.IsExternalLink(pdfDocumentNode.Id) ? PdfAction.CreateURI(pdfDocumentNode.Id) : PdfAction.CreateGoTo(pdfDocumentNode.Id));
            }

            if (pdfDocumentNode.HasChildren)
            {
                BuildPdfOutlines(outline, pdfDocumentNode.Children);
            }
        }
    }
}