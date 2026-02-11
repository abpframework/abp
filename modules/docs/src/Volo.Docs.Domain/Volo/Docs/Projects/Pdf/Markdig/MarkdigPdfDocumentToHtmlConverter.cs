using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Markdig;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Utils;

namespace Volo.Docs.Projects.Pdf.Markdig;

public class MarkdigPdfDocumentToHtmlConverter : IDocumentToHtmlConverter<PdfDocumentToHtmlConverterContext>, ITransientDependency
{
    public const string Type = "md";
    protected IOptions<DocsProjectPdfGeneratorOptions> Options { get; }  
    
    public MarkdigPdfDocumentToHtmlConverter(IOptions<DocsProjectPdfGeneratorOptions> options)
    {
        Options = options;
    }
    
    public virtual string Convert(PdfDocumentToHtmlConverterContext converterContext)
    {
        var htmlContent = Markdown.ToHtml(NormalizeContent(
                converterContext.Content,
                converterContext.PdfDocument,
                converterContext.DocumentParams), 
            GetPipeline(converterContext.PdfDocument));
        return NormalizeHtmlContent(htmlContent, converterContext.PdfDocument);
    }
    
    protected virtual MarkdownPipeline GetPipeline(PdfDocument pdfDocument)
    {
        return new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseBootstrap()
            .Use(new AnchorLinkResolverExtension(pdfDocument))
            .Build(); 
    }
    
    protected virtual string NormalizeContent(string content, PdfDocument pdfDocument, DocumentParams documentParams)
    {
        content = Regex.Replace(content, @"`{3,4}json\s*//\[doc-nav\][\s\S]*?`{3,4}", string.Empty, RegexOptions.IgnoreCase);
        content = SetContentTitle(content, pdfDocument, documentParams);
        if (Options.Value.DocumentContentNormalizer == null)
        {
            return content;
        }
        
        return Options.Value.DocumentContentNormalizer(content);
    }

    protected virtual string NormalizeHtmlContent(string htmlContent, PdfDocument pdfDocument)
    {
        htmlContent = WrapHtmlWithPageDiv(htmlContent, pdfDocument);
        htmlContent = ReplaceRelativeImageUrls(htmlContent, pdfDocument);
        if (Options.Value.HtmlContentNormalizer == null)
        {
            return htmlContent;
        }
        
        return Options.Value.HtmlContentNormalizer(htmlContent);
    }
    
    private string WrapHtmlWithPageDiv(string htmlContent, PdfDocument pdfDocument)
    {
        return $"<div class='page' id='{pdfDocument.Id}'>{htmlContent}</div>";
    }
    
    private string ReplaceRelativeImageUrls(string htmlContent, PdfDocument pdfDocument)
    {
        return Regex.Replace(htmlContent, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", delegate (Match match)
        {
            if (UrlHelper.IsExternalLink(match.Groups[2].Value))
            {
                return match.Value;
            }

            var rootUrl = UrlHelper.IsExternalLink(pdfDocument.Document.RawRootUrl)
                ? pdfDocument.Document.RawRootUrl.EnsureEndsWith('/')
                : Options.Value.BaseUrl.EnsureEndsWith('/') + pdfDocument.Document.RawRootUrl.TrimStart('/').EnsureEndsWith('/');
            var newImageSource = rootUrl +
                                 (pdfDocument.Document.LocalDirectory.IsNullOrEmpty() ? "" : pdfDocument.Document.LocalDirectory.TrimStart('/').EnsureEndsWith('/')) +
                                 match.Groups[2].Value.TrimStart('/');

            return match.Groups[1] + " src=\"" + HttpUtility.HtmlEncode(newImageSource) + "\" " + match.Groups[3];

        }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
    }

    private static string SetContentTitle(string content, PdfDocument pdfDocument, DocumentParams documentParams)
    {
        if (pdfDocument.RenderParameters.IsNullOrEmpty())
        {
            return content;
        }
        
        var titleLine = content.Split(Environment.NewLine).FirstOrDefault(x => x.TrimStart().StartsWith("#"));
        if (titleLine == null)
        {
            return content;
        }
        
        var paramValues = pdfDocument.RenderParameters.Select(x =>
        {
            var documentParam = documentParams.Parameters.FirstOrDefault(p => p.Name == x.Key);
            return $"{documentParam?.Values[x.Value] ?? x.Value}";
        });

        var newTitle = $"{titleLine.Trim()} ({string.Join(", ", paramValues)})";
        return content.Replace(titleLine, newTitle);
    }
}