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

        private const string NewLinkFormat = "[{0}](/documents/{1}/{2}/{3}/{4})";

        private const string MarkdownLinkRegExp = @"\[([^)]+)\]\(([^)]+." + Type + @")\)";

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

            return Regex.Replace(content, MarkdownLinkRegExp, delegate (Match match)
                {
                    var displayText = match.Groups[1].Value;
                    var documentName = RemoveFileExtensionIfLocalUrl(match.Groups[2].Value);
                    return string.Format(NewLinkFormat, displayText, projectShortName, version,
                        documentLocalDirectory.TrimStart('/').TrimEnd('/'), documentName);
                });
        }

        private static string RemoveFileExtensionIfLocalUrl(string documentName)
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

            if (IsRemoteUrl(documentName))
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
