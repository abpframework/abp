using System;
using System.Text.RegularExpressions;
using Volo.Docs.Utils;

namespace Volo.Docs.HtmlConverting
{
    public static class HtmlNormalizer
    {
        public static string ReplaceImageSources(string content, string documentRawRootUrl, string localDirectory)
        {
            if (content == null)
            {
                return null;
            }

            content = Regex.Replace(content, @"(<img\s+[^>]*)src=""([^""]*)""([^>]*>)", delegate (Match match)
                {
                    if (UrlHelper.IsExternalLink(match.Groups[2].Value))
                    {
                        return match.Value;
                    }

                    var newImageSource = documentRawRootUrl.EnsureEndsWith('/') +
                                         (localDirectory.IsNullOrEmpty() ? "" : localDirectory.TrimStart('/').EnsureEndsWith('/')) +
                                         match.Groups[2].Value.TrimStart('/');

                    return match.Groups[1] + " src=\"" + newImageSource + "\" " + match.Groups[3];

                }, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            return content;
        }

        public static string ReplaceCodeBlocksLanguage(string content, string currentLanguage, string newLanguage)
        {
            return Regex.Replace(content, "<code class=\"" + currentLanguage + "\">", "<code class=\"" + newLanguage + "\">", RegexOptions.IgnoreCase);
        }
    }
}