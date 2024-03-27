using System;
using System.Collections.Generic;
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
        private readonly LinkGenerator _linkGenerator;

        public MarkdownDocumentToHtmlConverter(IMarkdownConverter markdownConverter,
            IOptions<DocsUiOptions> urlOptions, LinkGenerator linkGenerator)
        {
            _markdownConverter = markdownConverter;
            _linkGenerator = linkGenerator;
            _uiOptions = urlOptions.Value;
        }

        private const string MdLinkFormat = "[{0}]({1})";
        private const string MarkdownLinkRegExp = @"\[(.*?)\]\(((.*?)(\?(.*?))*?)\)";
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
            return Regex.Replace(content, MarkdownLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[3].Value;

                var hashPart = "";
                if (link.Contains("#"))
                {
                    var linkSplitted = link.Split("#");
                    link = linkSplitted[0];
                    hashPart = linkSplitted[1];
                }

                if (UrlHelper.IsExternalLink(link))
                {
                    return match.Value;
                }

                if (!link.EndsWith(".md"))
                {
                    return match.Value;
                }

                var displayText = match.Groups[1].Value;

                var documentName = RemoveFileExtension(link);

                var hasUrlParameter = match.Groups.Count > 3 && !match.Groups[4].Value.IsNullOrEmpty();
                if (hasUrlParameter)
                {
                    documentName += match.Groups[4].Value;
                }

                var documentLocalDirectoryNormalized = documentLocalDirectory.TrimStart('/').TrimEnd('/');
                if (!string.IsNullOrWhiteSpace(documentLocalDirectoryNormalized))
                {
                    documentLocalDirectoryNormalized = "/" + documentLocalDirectoryNormalized;
                }

                if (!string.IsNullOrEmpty(hashPart))
                {
                    documentName += $"#{hashPart}";
                }

                return string.Format(
                    MdLinkFormat,
                    displayText,
                    GenerateUrl(languageCode, $"{version}{documentLocalDirectoryNormalized}", documentName, projectShortName)
                );
            });
        }

        private string NormalizeAnchorLinks(string projectShortName, string version, string documentLocalDirectory,
            string languageCode, string normalized)
        {
            return Regex.Replace(normalized, AnchorLinkRegExp, delegate (Match match)
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
                    GenerateUrl(languageCode, $"{version}{documentLocalDirectoryNormalized}", documentName, projectShortName)
                );
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
        
        private string GenerateUrl(string languageCode, string version, string documentName, string projectShortName)
        {
            var routeValues = new Dictionary<string, object> {
                { nameof(IndexModel.LanguageCode), languageCode },
                { nameof(IndexModel.Version), version },
                { nameof(IndexModel.DocumentName), documentName }
            };

            if (!_uiOptions.SingleProjectMode.Enable)
            {
                routeValues.Add(nameof(IndexModel.ProjectName), projectShortName);
            }

            return _linkGenerator.GetPathByPage("/Documents/Project/Index", values: routeValues);
        }
    }
}
