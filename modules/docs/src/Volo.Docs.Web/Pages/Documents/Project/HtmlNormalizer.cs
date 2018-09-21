using System;
using System.Text.RegularExpressions;

namespace Volo.Docs.Pages.Documents.Project
{
    public static class HtmlNormalizer
    {
        //todo: fix this for html content.
        public static string NormalizeLinks(string content, string projectShortName, string version)
        {
            //var linkRegex = new Regex(@"\(([^)]+.md)\)", RegexOptions.Multiline);
            //var matches = linkRegex.Matches(content);
            //foreach (Match match in matches)
            //{
            //    var mdFile = match.Value;
            //    content = content.Replace(mdFile, "(/documents/" + projectShortName + "/" + version + "/" + mdFile.Replace("(", "").Replace(")", "").Replace(".md", "") + ")");
            //}

            //return content;

            return content;
        }

        public static string NormalizeImages(string content, string documentRawRootUrl, string localDirectory)
        {
            content = Regex.Replace(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", delegate (Match match)
                {
                    var newImageSource = documentRawRootUrl.EnsureEndsWith('/') +
                                         (localDirectory.IsNullOrEmpty() ? "" : localDirectory.TrimStart('/').EnsureEndsWith('/')) +
                                         match.Groups[2].Value.TrimStart('/');
                    return match.Groups[1] + " src=\"" + newImageSource + "\" " + match.Groups[3];

                }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);


            return content;
        }
    }
}