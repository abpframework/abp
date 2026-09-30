using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using Shouldly;
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
        var html = new TestPdfSharpHtmlToPdfRenderer().Preserve(
            "<style>a::after { content: '&amp;'; }</style><a href='?a=1&amp;b=2' title='&quot;'>Tips &amp; Tricks &lt;T&gt;</a>");

        html.ShouldBe(
            "<style>a::after { content: '&amp;'; }</style><a href='?a=1&amp;b=2' title='&quot;'>Tips " +
            "<span style=\"word-break: normal\">&amp;</span> Tricks " +
            "<span style=\"word-break: normal\">&lt;</span>T<span style=\"word-break: normal\">&gt;</span></a>");
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
        public string Preserve(string html)
        {
            return PreserveHtmlEntities(html);
        }
    }
}
