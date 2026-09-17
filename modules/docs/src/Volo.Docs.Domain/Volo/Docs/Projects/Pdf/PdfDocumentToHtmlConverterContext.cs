using Volo.Docs.Documents;

namespace Volo.Docs.Projects.Pdf;

public class PdfDocumentToHtmlConverterContext
{
    public string Content { get; set; }
    public PdfDocument PdfDocument { get; set; }
    
    public DocumentParams DocumentParams { get; set; }
    
    public PdfDocumentToHtmlConverterContext(string content, PdfDocument pdfDocument, DocumentParams documentParams)
    {
        Content = content;
        PdfDocument = pdfDocument;
        DocumentParams = documentParams;
    }
}