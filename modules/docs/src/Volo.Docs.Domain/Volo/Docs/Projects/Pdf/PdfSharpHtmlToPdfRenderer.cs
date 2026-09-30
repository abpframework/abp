using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.PdfSharp.FontResolution;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Utils;
using PdfSharpDocument = PdfSharp.Pdf.PdfDocument;

namespace Volo.Docs.Projects.Pdf;

public class PdfSharpHtmlToPdfRenderer : IHtmlToPdfRenderer, ITransientDependency
{
    private static readonly Regex HtmlTagRegex = new(@"<[^>]*>", RegexOptions.Compiled);
    private static readonly Regex HtmlEntityRegex = new(@"&(?:#[0-9]+|#x[0-9a-fA-F]+|[a-zA-Z][a-zA-Z0-9]*);", RegexOptions.Compiled);
    private static readonly Regex PageDivRegex = new(@"<div\s+class=['""]page['""](?:\s+id=['""](?<id>[^'""]+)['""])?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public virtual Task<Stream> RenderAsync(string title, string html, List<PdfDocument> documents)
    {
        var pdfDocument = new PdfSharpDocument();
        pdfDocument.Info.Title = title;

        var config = CreatePdfGenerateConfig();
        var cssData = GetCssData();
        var elementPages = new Dictionary<string, int>();
        var pendingLinks = new List<PendingDocumentLink>();

        html = PreserveHtmlEntities(html);

        // HtmlRenderer ignores "page-break-after", so every document is laid out separately to start on a new page.
        foreach (var pageHtml in SplitHtmlIntoPages(html))
        {
            AddPages(pdfDocument, pageHtml, config, cssData, elementPages, pendingLinks);
        }

        foreach (var pendingLink in pendingLinks)
        {
            var targetPageIndex = FindPageIndex(elementPages, pendingLink.AnchorId);
            if (targetPageIndex.HasValue && targetPageIndex.Value != pendingLink.PageIndex)
            {
                pdfDocument.Pages[pendingLink.PageIndex].AddDocumentLink(pendingLink.Rectangle, targetPageIndex.Value + 1);
            }
        }

        AddOutlines(pdfDocument, pdfDocument.Outlines, documents, elementPages);

        var pdfStream = new MemoryStream();
        pdfDocument.Save(pdfStream, false);
        pdfStream.Position = 0;
        return Task.FromResult<Stream>(pdfStream);
    }

    protected virtual PdfGenerateConfig CreatePdfGenerateConfig()
    {
        var config = new PdfGenerateConfig
        {
            PageSize = PageSize.A4
        };
        config.SetMargins(36);
        return config;
    }

    protected virtual CssData GetCssData()
    {
        return null;
    }

    protected virtual void ConfigureFontResolver()
    {
        // The strict strategy falls back to a Latin-only font when a style is missing (e.g. a CJK font without a bold face).
        if (GlobalFontSettings.FontResolver is FontResolver fontResolver)
        {
            fontResolver.ResolveStrategy = FontResolveStrategy.Closest;
        }
    }

    protected virtual string PreserveHtmlEntities(string html)
    {
        // HtmlRenderer splits "word-break: break-all" text into single characters before decoding it, so "&amp;" would be printed as is.
        var builder = new StringBuilder(html.Length);
        var index = 0;
        var isRawText = false;
        foreach (Match tag in HtmlTagRegex.Matches(html))
        {
            AppendText(html.Substring(index, tag.Index - index));
            builder.Append(tag.Value);
            index = tag.Index + tag.Length;

            if (tag.Value.StartsWith("<style", StringComparison.OrdinalIgnoreCase) || tag.Value.StartsWith("<script", StringComparison.OrdinalIgnoreCase))
            {
                isRawText = true;
            }
            else if (tag.Value.StartsWith("</style", StringComparison.OrdinalIgnoreCase) || tag.Value.StartsWith("</script", StringComparison.OrdinalIgnoreCase))
            {
                isRawText = false;
            }
        }

        AppendText(html.Substring(index));
        return builder.ToString();

        void AppendText(string text)
        {
            builder.Append(isRawText ? text : HtmlEntityRegex.Replace(text, "<span style=\"word-break: normal\">$0</span>"));
        }
    }

    protected virtual List<string> SplitHtmlIntoPages(string html)
    {
        var matches = PageDivRegex.Matches(html);
        if (matches.Count <= 1)
        {
            return [html];
        }

        var bodyEndIndex = html.LastIndexOf("</body>", StringComparison.OrdinalIgnoreCase);
        if (bodyEndIndex < matches[^1].Index)
        {
            bodyEndIndex = html.Length;
        }

        var prefix = html.Substring(0, matches[0].Index);
        var suffix = html.Substring(bodyEndIndex);
        var pages = new List<string>();
        for (var i = 0; i < matches.Count; i++)
        {
            var endIndex = i + 1 < matches.Count ? matches[i + 1].Index : bodyEndIndex;
            pages.Add(prefix + html.Substring(matches[i].Index, endIndex - matches[i].Index) + suffix);
        }

        return pages;
    }

    protected virtual void AddPages(
        PdfSharpDocument pdfDocument,
        string html,
        PdfGenerateConfig config,
        CssData cssData,
        Dictionary<string, int> elementPages,
        List<PendingDocumentLink> pendingLinks)
    {
        var pageSize = PageSizeConverter.ToSize(config.PageSize);
        if (config.PageOrientation == PageOrientation.Landscape)
        {
            pageSize = new XSize(pageSize.Height, pageSize.Width);
        }

        var contentSize = new XSize(
            pageSize.Width - config.MarginLeft - config.MarginRight,
            pageSize.Height - config.MarginTop - config.MarginBottom);

        using var container = new HtmlContainer();
        ConfigureFontResolver();
        container.Location = new XPoint(config.MarginLeft, config.MarginTop);
        container.MaxSize = new XSize(contentSize.Width, 0);
        container.SetHtml(html, cssData);
        container.PageSize = contentSize;
        container.MarginTop = config.MarginTop;
        container.MarginBottom = config.MarginBottom;
        container.MarginLeft = config.MarginLeft;
        container.MarginRight = config.MarginRight;

        using (var measure = XGraphics.CreateMeasureContext(contentSize, XGraphicsUnit.Point, XPageDirection.Downwards))
        {
            container.PerformLayout(measure);
        }

        var firstPageIndex = pdfDocument.PageCount;
        for (var offset = 0.0; offset > -container.ActualSize.Height; offset -= contentSize.Height)
        {
            var page = pdfDocument.AddPage();
            page.Width = XUnit.FromPoint(pageSize.Width);
            page.Height = XUnit.FromPoint(pageSize.Height);
            using var graphics = XGraphics.FromPdfPage(page);
            graphics.IntersectClip(new XRect(0, 0, page.Width.Point, page.Height.Point));
            container.ScrollOffset = new XPoint(0, offset);
            container.PerformPaint(graphics);
        }

        int GetPageIndex(XRect rectangle)
        {
            var index = firstPageIndex + (int)((rectangle.Top - config.MarginTop) / contentSize.Height);
            return Math.Clamp(index, firstPageIndex, pdfDocument.PageCount - 1);
        }

        foreach (Match match in PageDivRegex.Matches(html))
        {
            var id = match.Groups["id"].Value;
            if (id.IsNullOrEmpty())
            {
                continue;
            }

            var rectangle = container.GetElementRectangle(id);
            if (rectangle.HasValue)
            {
                elementPages.TryAdd(id, GetPageIndex(rectangle.Value));
            }
        }

        foreach (var link in container.GetLinks())
        {
            var pageIndex = GetPageIndex(link.Rectangle);
            var top = link.Rectangle.Top - contentSize.Height * (pageIndex - firstPageIndex);
            var rectangle = new PdfRectangle(new XRect(
                link.Rectangle.Left,
                pageSize.Height - top - link.Rectangle.Height,
                link.Rectangle.Width,
                link.Rectangle.Height));

            if (!link.IsAnchor)
            {
                if (!link.Href.IsNullOrWhiteSpace())
                {
                    pdfDocument.Pages[pageIndex].AddWebLink(rectangle, link.Href);
                }

                continue;
            }

            if (link.AnchorId.IsNullOrWhiteSpace())
            {
                continue;
            }

            var targetRectangle = container.GetElementRectangle(link.AnchorId);
            if (targetRectangle.HasValue)
            {
                var targetPageIndex = GetPageIndex(targetRectangle.Value);
                if (targetPageIndex != pageIndex)
                {
                    pdfDocument.Pages[pageIndex].AddDocumentLink(rectangle, targetPageIndex + 1);
                }

                continue;
            }

            pendingLinks.Add(new PendingDocumentLink(pageIndex, rectangle, link.AnchorId));
        }
    }

    protected virtual void AddOutlines(
        PdfSharpDocument pdfDocument,
        PdfOutlineCollection outlines,
        List<PdfDocument> documents,
        Dictionary<string, int> elementPages)
    {
        foreach (var document in documents)
        {
            if (document.IgnoreOnOutline)
            {
                continue;
            }

            var pageIndex = FindPageIndex(elementPages, document.Id);
            var outline = pageIndex.HasValue
                ? outlines.Add(document.Title, pdfDocument.Pages[pageIndex.Value])
                : outlines.Add(document.Title, null);

            if (!pageIndex.HasValue && UrlHelper.IsExternalLink(document.Id))
            {
                var action = new PdfDictionary(pdfDocument);
                action.Elements.SetName("/S", "/URI");
                action.Elements.SetString("/URI", document.Id);
                outline.Elements["/A"] = action;
            }

            if (document.HasChildren)
            {
                AddOutlines(pdfDocument, outline.Outlines, document.Children, elementPages);
            }
        }
    }

    protected virtual int? FindPageIndex(Dictionary<string, int> elementPages, string id)
    {
        if (id.IsNullOrWhiteSpace())
        {
            return null;
        }

        if (elementPages.TryGetValue(id, out var pageIndex))
        {
            return pageIndex;
        }

        var documentId = id.Split('#').FirstOrDefault();
        return documentId != null && elementPages.TryGetValue(documentId, out pageIndex) ? pageIndex : null;
    }

    protected record PendingDocumentLink(int PageIndex, PdfRectangle Rectangle, string AnchorId);
}
