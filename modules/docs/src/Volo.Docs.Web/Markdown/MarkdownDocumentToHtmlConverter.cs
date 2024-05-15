using System;
using System.Collections.Generic;
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
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<DocsUrlNormalizerContext, string> _urlNormalizer;

        public MarkdownDocumentToHtmlConverter(IMarkdownConverter markdownConverter,
            IOptions<DocsUiOptions> urlOptions, IDocsLinkGenerator docsLinkGenerator, 
            IServiceProvider serviceProvider)
        {
            _markdownConverter = markdownConverter;
            _docsLinkGenerator = docsLinkGenerator;
            _serviceProvider = serviceProvider;
            _uiOptions = urlOptions.Value;
            _urlNormalizer = _uiOptions.UrlNormalizer ?? (context => context.Url);
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
            return Regex.Replace(content, MarkdownLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[3].Value;
                var displayText = match.Groups[1].Value;

                var hashPart = "";
                var linkPart = link;
                if (link.Contains("#"))
                {
                    var linkSplitted = link.Split("#");
                    linkPart = linkSplitted[0];
                    hashPart = linkSplitted[1];
                }
                
                var documentName = RemoveFileExtension(linkPart);

                if (UrlHelper.IsExternalLink(link) || !linkPart.EndsWith(".md"))
                {
                    return NormalizeLink(displayText, MdLinkFormat, link, projectShortName,
                        version,
                        documentName,
                        languageCode);
                }
                

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
                
                return NormalizeLink(
                    displayText,
                    MdLinkFormat,
                    _docsLinkGenerator.GenerateLink(projectShortName, languageCode, $"{version}{documentLocalDirectoryNormalized}", documentName),
                    projectShortName,
                    version,
                    documentName,
                    languageCode
                );
            });
        }

        private string NormalizeAnchorLinks(string projectShortName, string version, string documentLocalDirectory,
            string languageCode, string normalized)
        {
            return Regex.Replace(normalized, AnchorLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[1].Value;
                var displayText = match.Groups[2].Value;
                var documentName = RemoveFileExtension(link);

                if (UrlHelper.IsExternalLink(link))
                {
                    var documentLocalDirectoryNormalized = documentLocalDirectory.TrimStart('/').TrimEnd('/');
                    if (!string.IsNullOrWhiteSpace(documentLocalDirectoryNormalized))
                    {
                        documentLocalDirectoryNormalized = "/" + documentLocalDirectoryNormalized;
                    }
                    
                    link = _docsLinkGenerator.GenerateLink(projectShortName, languageCode, $"{version}{documentLocalDirectoryNormalized}", documentName);
                }
                
                return NormalizeLink(displayText, MdLinkFormat, link,projectShortName,
                    version,
                    documentName,
                    languageCode);
            });
        }
        
        private string NormalizeLink(string displayText, string linkFormat, string link, string projectName, string version, string documentName, string languageCode)
        {
            return string.Format(linkFormat, displayText, _urlNormalizer(new DocsUrlNormalizerContext {
                Url = link,
                ProjectName = projectName,
                Version = version,
                DocumentName = documentName,
                LanguageCode = languageCode,
                ServiceProvider = _serviceProvider
            }));
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
