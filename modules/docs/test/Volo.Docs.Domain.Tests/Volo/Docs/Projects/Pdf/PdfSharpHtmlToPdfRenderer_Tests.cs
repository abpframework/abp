using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using HtmlAgilityPack;
using Shouldly;
using SkiaSharp;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Docs.Projects.Pdf;

public class PdfSharpHtmlToPdfRenderer_Tests : DocsDomainTestBase
{
    private readonly IHtmlToPdfRenderer _htmlToPdfRenderer;

    public PdfSharpHtmlToPdfRenderer_Tests()
    {
        _htmlToPdfRenderer = GetRequiredService<IHtmlToPdfRenderer>();
    }

    [Fact]
    public void Should_Use_PdfSharp_Renderer_By_Default()
    {
        _htmlToPdfRenderer.ShouldBeOfType<PdfSharpHtmlToPdfRenderer>();
    }

    [Fact]
    public async Task Should_Start_Each_Document_On_A_New_Page()
    {
        var pdf = await RenderAsync(
            "<div class='page' id='first'><h1>First</h1><p>Short</p></div>" +
            "<div class='page' id='second'><h1>Second</h1><p>Short</p></div>",
            [
                new PdfDocument { Title = "First", Id = "first" },
                new PdfDocument { Title = "Second", Id = "second" }
            ]);

        pdf.PageCount.ShouldBe(2);
        GetOutlines(pdf).ShouldBe(["First:1", "Second:2"]);
    }

    [Fact]
    public async Task Should_Add_Outlines_For_Nested_And_External_Documents()
    {
        var pdf = await RenderAsync(
            "<div class='page' id='parent'><h1>Parent</h1></div>" +
            "<div class='page' id='child'><h1>Child</h1></div>" +
            "<div class='page' id='hidden'><h1>Hidden</h1></div>",
            [
                new PdfDocument
                {
                    Title = "Parent",
                    Id = "parent",
                    Children =
                    [
                        new PdfDocument { Title = "Child", Id = "child" },
                        new PdfDocument { Title = "Hidden", Id = "hidden", IgnoreOnOutline = true }
                    ]
                },
                new PdfDocument { Title = "External", Id = "https://abp.io" }
            ]);

        pdf.PageCount.ShouldBe(3);
        GetOutlines(pdf).ShouldBe(["Parent:1", "  Child:2", "External:https://abp.io"]);
    }

    [Fact]
    public async Task Should_Link_To_Other_Documents_And_Web_Pages()
    {
        var pdf = await RenderAsync(
            "<div class='page' id='first'><a href='#second'>Second</a> <a href='#second#details'>Details</a> <a href='https://abp.io'>ABP</a> <a href='#'>Empty</a></div>" +
            "<div class='page' id='second'><h1>Second</h1></div>",
            [
                new PdfDocument { Title = "First", Id = "first" },
                new PdfDocument { Title = "Second", Id = "second" }
            ]);

        var annotations = GetAnnotations(pdf, pdf.Pages[0]);
        annotations.Where(x => x.StartsWith("page:")).ShouldBe(["page:2", "page:2"]);
        annotations.Where(x => x.StartsWith("uri:")).ShouldBe(["uri:https://abp.io"]);
        GetAnnotations(pdf, pdf.Pages[1]).ShouldBeEmpty();
    }

    [Fact]
    public void Should_Keep_Html_Entities_As_Single_Words()
    {
        var html = CreateTestRenderer().PreserveEntities(
            "<style>a::after { content: '&amp;'; }</style><a href='?a=1&amp;b=2' title='&quot;'>Tips &amp; Tricks &lt;T&gt;</a>");

        html.ShouldBe(
            "<style>a::after { content: '&amp;'; }</style><a href='?a=1&amp;b=2' title='&quot;'>Tips " +
            "<span style=\"word-break: normal\">&amp;</span> Tricks " +
            "<span style=\"word-break: normal\">&lt;</span>T<span style=\"word-break: normal\">&gt;</span></a>");
    }

    [Fact]
    public async Task Should_Link_To_An_Anchor_On_The_Same_Page()
    {
        var pdf = await RenderAsync(
            "<div class='page' id='first'><a href='#details'>Details</a><h2 id='details'>Details</h2></div>",
            [new PdfDocument { Title = "First", Id = "first" }]);

        GetAnnotations(pdf, pdf.Pages[0]).ShouldBe(["page:1"]);
    }

    [Fact]
    public async Task Should_Link_To_An_Anchor_Written_By_The_Markdown_Converter()
    {
        var pdf = await RenderAsync(
            "<div class='page' id='first'><a href='##details'>Details</a><h2 id='details'>Details</h2></div>",
            [new PdfDocument { Title = "First", Id = "first" }]);

        GetAnnotations(pdf, pdf.Pages[0]).ShouldBe(["page:1"]);
    }

    [Fact]
    public async Task Should_Keep_Every_Line_Of_A_Wrapped_Link_Clickable()
    {
        var pdf = await RenderAsync(
            $"<div class='page' id='first'><a href='https://abp.io'>{string.Join(" ", Enumerable.Repeat("LINK", 60))}</a></div>",
            [new PdfDocument { Title = "First", Id = "first" }]);

        GetAnnotations(pdf, pdf.Pages[0]).Count.ShouldBe(60);
    }

    [Fact]
    public void Should_Split_Multi_Word_Links_Only()
    {
        var renderer = CreateTestRenderer();

        renderer.SplitLinks("<a href='#a'>Tips &amp; Tricks</a>").ShouldBe("<a href='#a'>Tips</a> <a href='#a'>&amp;</a> <a href='#a'>Tricks</a>");
        renderer.SplitLinks("<a href='#a'>Single</a>").ShouldBe("<a href='#a'>Single</a>");
        renderer.SplitLinks("<a id='x' href='#a'>Two words</a>").ShouldBe("<a id='x' href='#a'>Two words</a>");
        renderer.SplitLinks("<a href='#a'><code>Two words</code></a>").ShouldBe("<a href='#a'><code>Two words</code></a>");
    }

    [Fact]
    public void Should_Replace_Svg_Images_With_Links()
    {
        CreateTestRenderer().ReplaceImages("<img src=\"https://abp.io/diagram.svg?v=1\" alt=\"Diagram\" /><img src=\"https://abp.io/logo.png\" />")
            .ShouldBe("<a href=\"https://abp.io/diagram.svg?v=1\">Diagram</a><img src=\"https://abp.io/logo.png\">");
    }

    [Fact]
    public async Task Should_Not_Load_Local_Images()
    {
        var imagePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".png");
        using (var bitmap = new SKBitmap(3, 3))
        using (var data = bitmap.Encode(SKEncodedImageFormat.Png, 100))
        {
            await File.WriteAllBytesAsync(imagePath, data.ToArray());
        }

        try
        {
            var pdf = await RenderAsync(
                $"<div class='page' id='first'><img src='{new Uri(imagePath).AbsoluteUri}' /><img src='{imagePath}' /><p>Text</p></div>",
                [new PdfDocument { Title = "First", Id = "first" }]);

            var images = pdf.Pages[0].Resources.Elements.GetDictionary("/XObject")?.Elements.Values
                .Select(x => (PdfDictionary)((PdfReference)x).Value)
                .ToList() ?? [];
            images.ShouldNotContain(x => x.Elements.GetInteger("/Width") == 3);
        }
        finally
        {
            File.Delete(imagePath);
        }
    }

    [Fact]
    public void Should_Convert_Gif_Images()
    {
        var gif = System.Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");

        var image = CreateTestRenderer().CreatePdfImage(gif);

        image.PixelWidth.ShouldBe(1);
        image.PixelHeight.ShouldBe(1);
    }

    private TestPdfSharpHtmlToPdfRenderer CreateTestRenderer()
    {
        return new TestPdfSharpHtmlToPdfRenderer(GetRequiredService<IHttpClientFactory>());
    }

    private async Task<PdfSharp.Pdf.PdfDocument> RenderAsync(string content, List<PdfDocument> documents)
    {
        var html = $"<!DOCTYPE html><html><head><meta charset='utf-8' /></head><body>{content}</body></html>";
        await using var stream = await _htmlToPdfRenderer.RenderAsync("Test", html, documents);
        return PdfReader.Open(stream, PdfDocumentOpenMode.Import);
    }

    private static List<string> GetOutlines(PdfSharp.Pdf.PdfDocument pdf)
    {
        var result = new List<string>();
        AddOutlines(pdf, pdf.Outlines, string.Empty, result);
        return result;
    }

    private static void AddOutlines(PdfSharp.Pdf.PdfDocument pdf, PdfOutlineCollection outlines, string indent, List<string> result)
    {
        foreach (var outline in outlines)
        {
            var destination = outline.Elements.GetArray("/Dest");
            var target = destination != null
                ? GetPageNumber(pdf, destination.Elements[0]).ToString()
                : outline.Elements.GetDictionary("/A")?.Elements.GetString("/URI");
            result.Add($"{indent}{outline.Title}:{target}");
            AddOutlines(pdf, outline.Outlines, indent + "  ", result);
        }
    }

    private static List<string> GetAnnotations(PdfSharp.Pdf.PdfDocument pdf, PdfPage page)
    {
        var annotations = page.Elements.GetArray("/Annots");
        if (annotations == null)
        {
            return [];
        }

        return annotations.Elements
            .Select(x => (PdfDictionary)((PdfReference)x).Value)
            .Select(x => x.Elements.GetArray("/Dest") is { } destination
                ? $"page:{GetPageNumber(pdf, destination.Elements[0])}"
                : $"uri:{x.Elements.GetDictionary("/A")?.Elements.GetString("/URI")}")
            .ToList();
    }

    private static int GetPageNumber(PdfSharp.Pdf.PdfDocument pdf, PdfItem pageReference)
    {
        return pdf.Pages.Cast<PdfPage>().ToList().FindIndex(x => x.Reference == pageReference) + 1;
    }

    [DisableConventionalRegistration]
    private class TestPdfSharpHtmlToPdfRenderer : PdfSharpHtmlToPdfRenderer
    {
        public TestPdfSharpHtmlToPdfRenderer(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
        }

        public string PreserveEntities(string html)
        {
            return Transform(html, PreserveHtmlEntities);
        }

        public string SplitLinks(string html)
        {
            return Transform(html, SplitMultiWordLinks);
        }

        public string ReplaceImages(string html)
        {
            return Transform(html, ReplaceUnsupportedImages);
        }

        private static string Transform(string html, Action<HtmlDocument> action)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            action(htmlDocument);
            return htmlDocument.DocumentNode.OuterHtml;
        }

        public XImage CreatePdfImage(byte[] image)
        {
            return CreateImage(image);
        }
    }
}
