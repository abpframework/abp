﻿using System;
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

        /// <summary>
        /// Wraps an image with a tag that's clickable to open the image in a new browser tab.
        /// </summary>
        public static string WrapImagesWithinAnchors(string html)
        {
            try
            {
                return Regex.Replace(html, "<img.+?src=[\"'](.+?)[\"'].*?>", match =>
                {
                    var link = match.Groups[1].Value;
                    var imgTag = match.Groups[0].Value;
                    var title = GetTitleFromTag(imgTag);

                    return $"<a target = \"_blank\" rel=\"noopener noreferrer\" title=\"{title}\" href=\"{link}\"><img src=\"{link}\" alt=\"{title}\"></a>";
                });

            }
            catch
            {
                // ignored
                return html;
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