using System.Collections.Generic;
using System.Linq;
using Volo.Docs.Documents;
using Volo.Docs.Documents.Rendering;

namespace Volo.Docs.Projects.Pdf;

public class PdfDocument
{
    public Document Document { get; set; }
    public string Title { get; set; }
    public string Id { get; set; }
    public List<PdfDocument> Children { get; set; } = [];
    public DocumentRenderParameters RenderParameters { get; set; }
    public bool HasChildren => Children.Any();
    public bool IgnoreOnOutline { get; set; }
}