using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using SkiaSharp;
using TheArtOfDev.HtmlRenderer.Core;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.PdfSharp.FontResolution;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Docs.Utils;
using PdfSharpDocument = PdfSharp.Pdf.PdfDocument;

namespace Volo.Docs.Projects.Pdf;

public class PdfSharpHtmlToPdfRenderer : IHtmlToPdfRenderer, ITransientDependency
{
    private static readonly Regex HtmlEntityRegex = new(@"&(?:#[0-9]+|#x[0-9a-fA-F]+|[a-zA-Z][a-zA-Z0-9]*);", RegexOptions.Compiled);

    public ILogger<PdfSharpHtmlToPdfRenderer> Logger { get; set; }

    protected IHttpClientFactory HttpClientFactory { get; }

    public PdfSharpHtmlToPdfRenderer(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
        Logger = NullLogger<PdfSharpHtmlToPdfRenderer>.Instance;
    }

    public virtual Task<Stream> RenderAsync(string title, string html, List<PdfDocument> documents)
    {
        var pdfDocument = new PdfSharpDocument();
        pdfDocument.Info.Title = title;

        var config = CreatePdfGenerateConfig();
        var cssData = GetCssData();
        var elementPages = new Dictionary<string, int>();
        var pendingLinks = new List<PendingDocumentLink>();
        var images = new Dictionary<string, XImage>();

        var htmlDocument = new HtmlDocument();
        // HtmlRenderer draws a tab in code blocks as a missing glyph.
        htmlDocument.LoadHtml(html.Replace("\t", "    "));
        ReplaceUnsupportedImages(htmlDocument);
        SplitMultiWordLinks(htmlDocument);
        BreakLongWords(htmlDocument);
        PreserveHtmlEntities(htmlDocument);

        // HtmlRenderer ignores "page-break-after", so every document is laid out separately to start on a new page.
        foreach (var page in SplitIntoPages(htmlDocument))
        {
            AddPages(pdfDocument, page, config, cssData, elementPages, pendingLinks, images);
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

    protected virtual void ReplaceUnsupportedImages(HtmlDocument htmlDocument)
    {
        // PDFsharp can not draw SVG images, so they are rendered as links instead of an error icon.
        foreach (var image in htmlDocument.DocumentNode.Descendants("img").ToList())
        {
            var src = image.GetAttributeValue("src", string.Empty);
            var path = src.Split('?')[0];
            if (!path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var alt = HtmlEntity.DeEntitize(image.GetAttributeValue("alt", string.Empty));
            var link = htmlDocument.CreateElement("a");
            link.SetAttributeValue("href", src);
            link.AppendChild(htmlDocument.CreateTextNode(HtmlEntity.Entitize(alt.IsNullOrWhiteSpace() ? Path.GetFileName(path) : alt)));
            image.ParentNode.ReplaceChild(link, image);
        }
    }

    protected virtual void SplitMultiWordLinks(HtmlDocument htmlDocument)
    {
        // HtmlRenderer only reports the first line of a link, so every word gets its own link to keep wrapped links clickable.
        foreach (var link in htmlDocument.DocumentNode.Descendants("a").ToList())
        {
            if (link.ChildNodes.Count != 1 || link.FirstChild.NodeType != HtmlNodeType.Text || link.Attributes.Contains("id") || link.Attributes.Contains("name"))
            {
                continue;
            }

            var parts = SplitWords(link.FirstChild.InnerHtml);
            if (parts.Count(part => !part.IsNullOrWhiteSpace()) <= 1)
            {
                continue;
            }

            foreach (var part in parts)
            {
                if (part.IsNullOrWhiteSpace())
                {
                    link.ParentNode.InsertBefore(htmlDocument.CreateTextNode(part), link);
                    continue;
                }

                var wordLink = link.CloneNode(false);
                wordLink.AppendChild(htmlDocument.CreateTextNode(part));
                link.ParentNode.InsertBefore(wordLink, link);
            }

            link.Remove();
        }
    }

    protected virtual void BreakLongWords(HtmlDocument htmlDocument)
    {
        // HtmlRenderer does not support "overflow-wrap: break-word": a word wider than its line is clipped, and "word-break: break-all" breaks every word.
        // So only the words that can not fit are allowed to break anywhere.
        foreach (var textNode in htmlDocument.DocumentNode.Descendants().OfType<HtmlTextNode>().ToList())
        {
            if (textNode.ParentNode.Name is "style" or "script" or "title" || textNode.Ancestors("pre").Any())
            {
                continue;
            }

            var maxWordLength = GetMaxWordLength(textNode);
            var parts = SplitWords(textNode.Text);
            if (!parts.Any(part => IsLongWord(part, maxWordLength)))
            {
                continue;
            }

            foreach (var part in parts)
            {
                if (!IsLongWord(part, maxWordLength))
                {
                    textNode.ParentNode.InsertBefore(htmlDocument.CreateTextNode(part), textNode);
                    continue;
                }

                var span = htmlDocument.CreateElement("span");
                span.SetAttributeValue("style", "word-break: break-all");
                span.AppendChild(htmlDocument.CreateTextNode(part));
                textNode.ParentNode.InsertBefore(span, textNode);
            }

            textNode.Remove();
        }
    }

    protected virtual int GetMaxWordLength(HtmlNode textNode)
    {
        // A line of the default style holds about 90 characters, but table columns are not equally wide and code uses a wider font.
        var cell = textNode.Ancestors().FirstOrDefault(node => node.Name is "td" or "th");
        if (cell == null)
        {
            return 40;
        }

        var columnCount = cell.ParentNode.ChildNodes
            .Where(node => node.Name is "td" or "th")
            .Sum(node => Math.Max(node.GetAttributeValue("colspan", 1), 1));
        var maxLength = Math.Max(45 / columnCount, 6);

        return textNode.Ancestors("code").Any() ? maxLength * 3 / 4 : maxLength;
    }

    private static bool IsLongWord(string text, int maxLength)
    {
        return !text.IsNullOrWhiteSpace() && HtmlEntity.DeEntitize(text).Length > maxLength;
    }

    protected virtual void PreserveHtmlEntities(HtmlDocument htmlDocument)
    {
        // HtmlRenderer splits "word-break: break-all" text into single characters before decoding it, so "&amp;" would be printed as is.
        foreach (var textNode in htmlDocument.DocumentNode.Descendants().OfType<HtmlTextNode>().ToList())
        {
            if (textNode.ParentNode.Name is "style" or "script" or "title" || !HtmlEntityRegex.IsMatch(textNode.Text))
            {
                continue;
            }

            var index = 0;
            foreach (Match entity in HtmlEntityRegex.Matches(textNode.Text))
            {
                if (entity.Index > index)
                {
                    textNode.ParentNode.InsertBefore(htmlDocument.CreateTextNode(textNode.Text.Substring(index, entity.Index - index)), textNode);
                }

                var span = htmlDocument.CreateElement("span");
                span.SetAttributeValue("style", "word-break: normal");
                span.AppendChild(htmlDocument.CreateTextNode(entity.Value));
                textNode.ParentNode.InsertBefore(span, textNode);
                index = entity.Index + entity.Length;
            }

            if (index < textNode.Text.Length)
            {
                textNode.ParentNode.InsertBefore(htmlDocument.CreateTextNode(textNode.Text.Substring(index)), textNode);
            }

            textNode.Remove();
        }
    }

    protected virtual List<PdfPageHtml> SplitIntoPages(HtmlDocument htmlDocument)
    {
        var pageNodes = htmlDocument.DocumentNode.Descendants("div")
            .Where(node => node.HasClass("page") && !node.Ancestors("div").Any(ancestor => ancestor.HasClass("page")))
            .ToList();

        if (pageNodes.Count <= 1)
        {
            return [new PdfPageHtml(pageNodes.FirstOrDefault()?.Id, htmlDocument.DocumentNode.OuterHtml)];
        }

        // Every page is rendered with the rest of the layout, in place of the first page.
        const string placeholder = "docs-pdf-page-placeholder";
        pageNodes[0].ParentNode.InsertBefore(htmlDocument.CreateComment($"<!--{placeholder}-->"), pageNodes[0]);
        var pages = pageNodes.Select(node => (node.Id, node.OuterHtml)).ToList();
        pageNodes.ForEach(node => node.Remove());

        var layout = htmlDocument.DocumentNode.OuterHtml;
        return pages.Select(page => new PdfPageHtml(page.Id, layout.Replace($"<!--{placeholder}-->", page.OuterHtml))).ToList();
    }

    protected virtual List<string> SplitWords(string text)
    {
        var parts = new List<string>();
        var start = 0;
        for (var i = 1; i <= text.Length; i++)
        {
            if (i == text.Length || char.IsWhiteSpace(text[i]) != char.IsWhiteSpace(text[start]))
            {
                parts.Add(text.Substring(start, i - start));
                start = i;
            }
        }

        return parts;
    }

    protected virtual void AddPages(
        PdfSharpDocument pdfDocument,
        PdfPageHtml page,
        PdfGenerateConfig config,
        CssData cssData,
        Dictionary<string, int> elementPages,
        List<PendingDocumentLink> pendingLinks,
        Dictionary<string, XImage> images)
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
        container.ImageLoad += (_, args) => LoadImage(args, images);
        container.RenderError += (_, args) => Logger.LogWarning(args.Exception, "PDF rendering error: {Message}", args.Message);
        container.Location = new XPoint(config.MarginLeft, config.MarginTop);
        container.MaxSize = new XSize(contentSize.Width, 0);
        container.SetHtml(page.Html, cssData);
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
            var pdfPage = pdfDocument.AddPage();
            pdfPage.Width = XUnit.FromPoint(pageSize.Width);
            pdfPage.Height = XUnit.FromPoint(pageSize.Height);
            using var graphics = XGraphics.FromPdfPage(pdfPage);
            graphics.IntersectClip(new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point));
            container.ScrollOffset = new XPoint(0, offset);
            container.PerformPaint(graphics);
        }

        int GetPageIndex(XRect rectangle)
        {
            var index = firstPageIndex + (int)((rectangle.Top - config.MarginTop) / contentSize.Height);
            return Math.Clamp(index, firstPageIndex, pdfDocument.PageCount - 1);
        }

        double GetTopOnPage(XRect rectangle, int pageIndex)
        {
            return rectangle.Top - contentSize.Height * (pageIndex - firstPageIndex);
        }

        var pageRectangle = page.Id.IsNullOrEmpty() ? null : container.GetElementRectangle(page.Id);
        if (pageRectangle.HasValue)
        {
            elementPages.TryAdd(page.Id, GetPageIndex(pageRectangle.Value));
        }

        foreach (var link in container.GetLinks())
        {
            var pageIndex = GetPageIndex(link.Rectangle);
            var top = GetTopOnPage(link.Rectangle, pageIndex);
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

            // The Markdown converter writes "##id" for anchors in the same document.
            var anchorId = link.AnchorId.TrimStart('#');
            if (anchorId.IsNullOrWhiteSpace())
            {
                continue;
            }

            var targetRectangle = container.GetElementRectangle(anchorId);
            if (targetRectangle.HasValue)
            {
                var targetPageIndex = GetPageIndex(targetRectangle.Value);
                var targetPoint = new XPoint(targetRectangle.Value.Left, pageSize.Height - GetTopOnPage(targetRectangle.Value, targetPageIndex));
                pdfDocument.Pages[pageIndex].AddDocumentLink(rectangle, targetPageIndex + 1, targetPoint);
                continue;
            }

            pendingLinks.Add(new PendingDocumentLink(pageIndex, rectangle, anchorId));
        }
    }

    protected virtual void LoadImage(HtmlImageLoadEventArgs args, Dictionary<string, XImage> images)
    {
        if (args.Src.IsNullOrWhiteSpace() || args.Src.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // Only remote images are loaded, so a document can not read files from the server.
        if (!Uri.TryCreate(args.Src, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            Logger.LogWarning("The image {ImageUrl} is not an HTTP(S) URL and is not added to the PDF file.", args.Src);
            args.Handled = true;
            args.Callback();
            return;
        }

        if (!images.TryGetValue(args.Src, out var image))
        {
            try
            {
                var bytes = AsyncHelper.RunSync(() => DownloadImageAsync(uri));
                image = CreateImage(bytes);
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, "Could not load the image {ImageUrl} for the PDF file.", args.Src);
            }

            images[args.Src] = image;
        }

        // A failed image is not handed back to HtmlRenderer, so its own downloader can not load it without the limits above.
        args.Handled = true;
        if (image != null)
        {
            args.Callback(image);
        }
        else
        {
            args.Callback();
        }
    }

    protected virtual async Task<byte[]> DownloadImageAsync(Uri uri)
    {
        var client = HttpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(30);
        client.MaxResponseContentBufferSize = 20 * 1024 * 1024;
        return await client.GetByteArrayAsync(uri);
    }

    protected virtual XImage CreateImage(byte[] bytes)
    {
        try
        {
            return XImage.FromStream(new MemoryStream(bytes));
        }
        catch (Exception)
        {
            // PDFsharp reads PNG, JPEG and BMP only (and not every JPEG), so other images are converted to PNG. An animated GIF keeps its first frame.
            using var bitmap = SKBitmap.Decode(bytes) ?? throw new NotSupportedException("The image format is not supported.");
            using var data = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            return XImage.FromStream(new MemoryStream(data.ToArray()));
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

    protected record PdfPageHtml(string Id, string Html);

    protected record PendingDocumentLink(int PageIndex, PdfRectangle Rectangle, string AnchorId);
}
