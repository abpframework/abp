using System;
using System.Text;
using System.Text.RegularExpressions;
using CommonMark;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Formatting
{
    public class MarkdownDocumentConverter : IDocumentConverter, ITransientDependency
    {
        public const string Type = "md";

        private const string NewLinkFormat = "[{0}](/documents/{1}/{2}{3}/{4})";

        private const string MarkdownLinkRegExp = @"\[([^)]+)\]\(([^)]+." + Type + @")\)";

        private const string AnchorLinkRegExp = @"<a[^>]+href=\""(.*?)\""[^>]*>(.*)?</a>";

        public string Convert(string content)
        {
            return content == null ? null : CommonMarkConverter.Convert(Encoding.UTF8.GetString(Encoding.Default.GetBytes(content)));
        }

        public string NormalizeLinks(string content, string projectShortName, string version,
            string documentLocalDirectory)
        {
            if (content == null)
            {
                return null;
            }

            var normalized = Regex.Replace(content, MarkdownLinkRegExp, delegate (Match match)
               {
                   var displayText = match.Groups[1].Value;
                   var documentName = RemoveFileExtension(match.Groups[2].Value);
                   var documentLocalDirectoryNormalized = documentLocalDirectory.TrimStart('/').TrimEnd('/');
                   if (!string.IsNullOrWhiteSpace(documentLocalDirectoryNormalized))
                   {
                       documentLocalDirectoryNormalized = "/" + documentLocalDirectoryNormalized;
                   }

                   return string.Format(NewLinkFormat, displayText, projectShortName, version, documentLocalDirectoryNormalized, documentName);
               });

            normalized = Regex.Replace(normalized, AnchorLinkRegExp, delegate (Match match)
            {
                var link = match.Groups[1].Value;
                if (IsRemoteUrl(link))
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

                return string.Format(NewLinkFormat, displayText, projectShortName, version, documentLocalDirectoryNormalized, documentName);
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

        private static bool IsRemoteUrl(string url)
        {
            if (url == null)
            {
                return true;
            }

            try
            {
                return Regex.IsMatch(url, @"\A(https?|ftp)://(-\.)?([^\s/?\.#-]+\.?)+(/[^\s]*)?\z");
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
