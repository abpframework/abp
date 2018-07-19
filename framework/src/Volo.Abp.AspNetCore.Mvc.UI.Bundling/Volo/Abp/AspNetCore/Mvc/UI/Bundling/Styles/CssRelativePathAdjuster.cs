using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.Styles
{
    internal static class CssRelativePath
    {
        private static readonly Regex _rxUrl = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string Adjust(
            string cssFileContents, 
            string absoluteInputFilePath, 
            string absoluteOutputPath)
        {
            var matches = _rxUrl.Matches(cssFileContents);

            if (matches.Count <= 0)
            {
                return cssFileContents;
            }

            var cssDirectoryPath = Path.GetDirectoryName(absoluteInputFilePath);

            foreach (Match match in matches)
            {
                string quoteDelimiter = match.Groups[1].Value; //url('') vs url("")
                string relativePathToCss = match.Groups[2].Value;

                // Ignore root relative references
                if (relativePathToCss.StartsWith("/", StringComparison.Ordinal))
                    continue;

                //prevent query string from causing error
                var pathAndQuery = relativePathToCss.Split(new[] { '?' }, 2, StringSplitOptions.RemoveEmptyEntries);
                var pathOnly = pathAndQuery[0];
                var queryOnly = pathAndQuery.Length == 2 ? pathAndQuery[1] : string.Empty;

                string absolutePath = GetAbsolutePath(cssDirectoryPath, pathOnly);
                string serverRelativeUrl = MakeRelative(absoluteOutputPath, absolutePath);

                if (!string.IsNullOrEmpty(queryOnly))
                    serverRelativeUrl += "?" + queryOnly;

                string replace = string.Format("url({0}{1}{0})", quoteDelimiter, serverRelativeUrl);

                cssFileContents = cssFileContents.Replace(match.Groups[0].Value, replace);
            }

            return cssFileContents;
        }

        private static string GetAbsolutePath(string cssFilePath, string pathOnly)
        {
            return Path.GetFullPath(Path.Combine(cssFilePath, pathOnly));
        }

        private static readonly string _protocol = "file:///";
        private static string MakeRelative(string baseFile, string file)
        {
            if (string.IsNullOrEmpty(file))
                return file;

            Uri baseUri = new Uri(_protocol + baseFile, UriKind.RelativeOrAbsolute);
            Uri fileUri = new Uri(_protocol + file, UriKind.RelativeOrAbsolute);

            if (baseUri.IsAbsoluteUri)
            {
                return Uri.UnescapeDataString(baseUri.MakeRelativeUri(fileUri).ToString());
            }
            else
            {
                return baseUri.ToString();
            }
        }
    }
}
