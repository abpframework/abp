using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Documents;
using Volo.Docs.HtmlConverting;
using Volo.Docs.Projects;
using Volo.Docs.Utils;

namespace Volo.Docs.Markdown
{
    public class MarkdownDocumentToHtmlConverter : IDocumentToHtmlConverter, ITransientDependency
    {
        public const string Type = "md";

        private readonly IMarkdownConverter _markdownConverter;
        private readonly DocsUiOptions _uiOptions;

        public MarkdownDocumentToHtmlConverter(IMarkdownConverter markdownConverter,
            IOptions<DocsUiOptions> urlOptions)
        {
            _markdownConverter = markdownConverter;
            _uiOptions = urlOptions.Value;
        }

        private const string MdLinkFormat = "[{0}]({1}{2}/{3}/{4}{5}/{6})";
        private const string MarkdownLinkRegExp = @"\[(.*?)\]\((.*?)\)";
        private const string AnchorLinkRegExp = @"<a[^>]+href=\""(.*?)\""[^>]*>(.*)?</a>";
         
        public virtual string Convert(ProjectDto project, DocumentWithDetailsDto document, string version,
            string languageCode)
        {
            if (document.Content.IsNullOrEmpty())
            {
                return document.Content;
            }

            var content = NormalizeLinks(
                document.Content,
                project.ShortName,
                version,
                document.LocalDirectory,
                languageCode
            );

            return _markdownConverter.ConvertToHtml(content);
        }
        
        protected virtual string NormalizeLinks(
            string content,
            string projectShortName,
            string version,
            string documentLocalDirectory,
            string languageCode)
        {
            var normalized = Regex.Replace(content, MarkdownLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[2].Value;
                if (UrlHelper.IsExternalLink(link) || !link.EndsWith(".md"))
                {
                    return match.Value;
                }

                var displayText = match.Groups[1].Value;

                var documentName = RemoveFileExtension(link);
                var documentLocalDirectoryNormalized = documentLocalDirectory.TrimStart('/').TrimEnd('/');
                if (!string.IsNullOrWhiteSpace(documentLocalDirectoryNormalized))
                {
                    documentLocalDirectoryNormalized = "/" + documentLocalDirectoryNormalized;
                }

                return string.Format(
                    MdLinkFormat,
                    displayText,
                    _uiOptions.RoutePrefix,
                    languageCode,
                    projectShortName,
                    version,
                    documentLocalDirectoryNormalized,
                    documentName
                );
            });

            normalized = Regex.Replace(normalized, AnchorLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[1].Value;
                if (UrlHelper.IsExternalLink(link))
                {
                    return match.Value;
                }

                var displayText = match.Groups[2].Value;
                var documentName = RemoveFileExtension(link);
                var documentLocalDirectoryNormalized = documentLocalDirectory.TrimStart('/').TrimEnd('/');
                if (!string.IsNullOrWhiteSpace(documentLocalDirectoryNormalized))
                {
                    documentLocalDirectoryNormalized = "/" + documentLocalDirectoryNormalized;
                }

                return string.Format(
                    MdLinkFormat,
                    displayText,
                    _uiOptions.RoutePrefix,
                    languageCode,
                    projectShortName,
                    version,
                    documentLocalDirectoryNormalized,
                    documentName
                );
            });

            return normalized;
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
