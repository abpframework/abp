using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Pages.Documents.Project;
using Volo.Docs.Projects;
using Volo.Docs.Utils;

namespace Volo.Docs.Markdown
{
    public class MarkdownDocumentToHtmlConverter : IDocumentToHtmlConverter, ITransientDependency
    {
        public const string Type = "md";

        private readonly IMarkdownConverter _markdownConverter;
        private readonly DocsUiOptions _uiOptions;
        private readonly IDocsLinkGenerator _docsLinkGenerator;

        public MarkdownDocumentToHtmlConverter(IMarkdownConverter markdownConverter,
            IOptions<DocsUiOptions> urlOptions, IDocsLinkGenerator docsLinkGenerator)
        {
            _markdownConverter = markdownConverter;
            _docsLinkGenerator = docsLinkGenerator;
            _uiOptions = urlOptions.Value;
        }

        private const string MdLinkFormat = "[{0}]({1})";
        private const string MarkdownLinkRegExp = @"\[(.*?)\]\(((.*?)(\?(.*?))*?)\)";
        private const string AnchorLinkRegExp = @"<a[^>]+href=\""(.*?)\""[^>]*>(.*)?</a>";

        public virtual string Convert(ProjectDto project, DocumentWithDetailsDto document, string version,
            string languageCode, string projectShortName = null)
        {
            if (document.Content.IsNullOrEmpty())
            {
                return document.Content;
            }

            var content = NormalizeLinks(
                document.Content,
                _uiOptions.SingleProjectMode.Enable ? projectShortName : projectShortName ?? project.ShortName,
                version,
                document.LocalDirectory,
                !_uiOptions.MultiLanguageMode ? languageCode : languageCode ?? document.LanguageCode
            );

            var html = _markdownConverter.ConvertToHtml(content);

            return html;
            //  return HtmlNormalizer.WrapImagesWithinAnchors(html);
        }

        protected virtual string NormalizeLinks(
            string content,
            string projectShortName,
            string version,
            string documentLocalDirectory,
            string languageCode)
        {

            var normalized = NormalizeMdLinks(content, projectShortName, version, documentLocalDirectory, languageCode);

            normalized = NormalizeAnchorLinks(projectShortName, version, documentLocalDirectory, languageCode, normalized);

            return normalized;
        }

        private string NormalizeMdLinks(string content,
            string projectShortName,
            string version,
            string documentLocalDirectory,
            string languageCode)
        {
            return NormalizeLinksByRegexPattern(MarkdownLinkRegExp, 3, 1, content, projectShortName, version, documentLocalDirectory, languageCode);
        }

        private string NormalizeAnchorLinks(string projectShortName, string version, string documentLocalDirectory,
            string languageCode, string normalized)
        {
            return NormalizeLinksByRegexPattern(AnchorLinkRegExp, 1, 2, normalized, projectShortName, version, documentLocalDirectory, languageCode);
        }
        
        private string NormalizeLinksByRegexPattern(string regexPattern,
            int linkGroupIndex,
            int displayTextGroupIndex,
            string content,
            string projectShortName,
            string version,
            string documentLocalDirectory,
            string languageCode)
        {
            return Regex.Replace(content, regexPattern, delegate (Match match)
            {
                var link = match.Groups[linkGroupIndex].Value;
                var displayText = match.Groups[displayTextGroupIndex].Value;

                var hashPart = string.Empty;
                var linkPart = link;
                if (link.Contains('#'))
                {
                    var linkSplitted = link.Split('#');
                    linkPart = linkSplitted[0];
                    hashPart = linkSplitted[1];
                }
                
                var documentName = RemoveFileExtension(linkPart);
                
                var isFolder = !linkPart.IsNullOrWhiteSpace() && !Path.HasExtension(linkPart);
                
                var isMdFile = linkPart.EndsWith(".md");

                if (UrlHelper.IsExternalLink(link) || !(isMdFile || isFolder))
                {
                    return match.Value;
                }

                var documentLocalDirectoryNormalized = documentLocalDirectory.TrimStart('/').TrimEnd('/');
                if (!string.IsNullOrWhiteSpace(documentLocalDirectoryNormalized))
                {
                    documentLocalDirectoryNormalized = "/" + documentLocalDirectoryNormalized;
                }
                
                var hasUrlParameter = match.Groups.Count > 3 && !match.Groups[4].Value.IsNullOrEmpty();
                
                return string.Format(MdLinkFormat, displayText,
                    _docsLinkGenerator.GenerateLink(projectShortName, languageCode, $"{version}{documentLocalDirectoryNormalized}", documentName) 
                    + (hasUrlParameter ? match.Groups[4].Value : string.Empty)
                    + (hashPart.IsNullOrWhiteSpace() ? string.Empty : "#" + hashPart));
            });
        }

        private static string RemoveFileExtension(string documentName)
        {
            if (documentName == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(documentName))
            {
                return documentName;
            }

            if (!documentName.EndsWith(Type, StringComparison.OrdinalIgnoreCase))
            {
                return documentName;
            }

            return documentName.Left(documentName.Length - Type.Length - 1);
        }
    }
}
