using System;
using System.Collections.Generic;
using System.Text;
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
            var sb = new StringBuilder();
            var pattern = sb.Append("<code class=\"").Append(currentLanguage).Append("\">").ToString();
            sb.Clear();
            var replacement = sb.Append("<code class=\"").Append(newLanguage).Append("\">").ToString();

            return Regex.Replace(content, pattern, replacement, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Wraps an image with a tag that's clickable to open the image in a new browser tab.
        /// </summary>
        public static string WrapImagesWithinAnchors(string html)
        {
            try
            {
                var ignoredIndicies = GetIgnoredImageIndicies(html);

                return Regex.Replace(html, "<img.+?src=[\"'](.+?)[\"'].*?>", match =>
                {
                    if (ignoredIndicies != null && ignoredIndicies.Contains(match.Index))
                    {
                        return match.Value;
                    }

                    var link = match.Groups[1].Value;
                    var imgTag = match.Groups[0].Value;
                    var title = GetTitleFromTag(imgTag);

                    return $"<a target = \"_blank\" rel=\"noopener noreferrer\" title=\"{title}\" href=\"{link}\">{imgTag}</a>";
                });
            }
            catch
            {
                // ignored
                return html;
            }
        }

        private static List<int> GetIgnoredImageIndicies(string html)
        {
            return GetIgnoredImageIndicies(FindImgTagsWithinAnchor(html));
        }

        private static List<int> GetIgnoredImageIndicies(MatchCollection ignoredImages)
        {
            if (ignoredImages == null)
            {
                return null;
            }

            var ignoredImageIndicies = new List<int>(ignoredImages.Count);
            for (var i = 0; i < ignoredImages.Count; i++)
            {
                var ignoredImage = ignoredImages[i];
                var ignoredImgIndex = ignoredImage.Index +
                                      ignoredImage.Value.IndexOf("<img", StringComparison.InvariantCultureIgnoreCase);

                ignoredImageIndicies.Add(ignoredImgIndex);
            }

            return ignoredImageIndicies;
        }

        public static MatchCollection FindImgTagsWithinAnchor(string html)
        {
            try
            {
                return Regex.Matches(html, @"<a(?: [^<>]+)?>(?:(?!<\s*/\s*a\s*>).)*<img.*?\s*<\s*/\s*a\s*>");
            }
            catch
            {
                // ignored
                return null;
            }
        }

        private static string GetTitleFromTag(string imgTag)
        {
            if (string.IsNullOrWhiteSpace(imgTag))
            {
                return null;
            }

            var match = Regex.Match(imgTag, @"\stitle\s?\=\s?(\""|')(.+?)(\""|')", RegexOptions.Multiline);
            if (match.Success && match.Groups.Count > 2)
            {
                return match.Groups[2].ToString().Trim();
            }

            match = Regex.Match(imgTag, @"\salt\s*\=\s*(\""|')(.*?)(\""|')", RegexOptions.Multiline);
            if (match.Success && match.Groups.Count > 2)
            {
                return match.Groups[2].ToString().Trim();
            }

            return null;
        }
    }
}
