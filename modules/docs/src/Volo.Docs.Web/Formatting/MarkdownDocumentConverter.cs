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

        public string Convert(string content)
        {
            return CommonMarkConverter.Convert(Encoding.UTF8.GetString(Encoding.Default.GetBytes(content)));
        }

        public string NormalizeLinks(string content, string projectShortName, string version,
            string documentLocalDirectory)
        {
            return Regex.Replace(content, @"\[([^)]+)\]\(([^)]+.md)\)", delegate (Match match)
               {
                   var displayText = match.Groups[1].Value;
                   var documentName  = match.Groups[2].Value;
                   var newLink = string.Format(NewLinkFormat, displayText, projectShortName, version, documentLocalDirectory.TrimStart('/').TrimEnd('/'), documentName);
                   return newLink;
               });
        }

    }
}
