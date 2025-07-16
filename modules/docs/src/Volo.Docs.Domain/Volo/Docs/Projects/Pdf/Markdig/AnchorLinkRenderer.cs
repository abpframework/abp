using System;
using System.Linq;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;
using Volo.Docs.Utils;

namespace Volo.Docs.Projects.Pdf.Markdig;

public class AnchorLinkRenderer : LinkInlineRenderer
{
    private readonly PdfDocument _document;
    
    public AnchorLinkRenderer(PdfDocument document)
    {
        _document = document;
    }
    
    protected override void Write(HtmlRenderer renderer, LinkInline link)
    {
        if (UrlHelper.IsExternalLink(link.Url) || link.Url.IsNullOrWhiteSpace() || link.IsImage)
        {
            base.Write(renderer, link);
            return;
        }

        link.GetDynamicUrl = () => "#" + ResolveRelativeMarkdownPath(_document.Document.Name, link.Url)
            .Replace(".md", string.Empty).Replace("/", "-").Replace(" ", "-").ToLower();
        base.Write(renderer, link);
    }
    
    private string ResolveRelativeMarkdownPath(string currentPath, string relativePath)
    {
        currentPath = currentPath.EnsureStartsWith('/');
        relativePath = relativePath.EnsureStartsWith('/');
        
        var currentDir = currentPath.Substring(0, currentPath.LastIndexOf('/'));

        var baseParts = currentDir.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
        var relativeParts = relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in relativeParts)
        {
            if (part == "..")
            {
                if (baseParts.Count > 0)
                {
                    baseParts.RemoveAt(baseParts.Count - 1);
                }
            }
            else if (part != ".")
            {
                baseParts.Add(part);
            }
        }

        return string.Join("/", baseParts);
    } 
}