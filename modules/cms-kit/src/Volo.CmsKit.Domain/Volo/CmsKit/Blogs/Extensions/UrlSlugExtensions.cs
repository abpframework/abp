using System.Text.RegularExpressions;
using Unidecode.NET;

namespace Volo.CmsKit.Blogs.Extensions
{
    public static class UrlSlugExtensions
    {
        public static string NormalizeAsUrlSlug(this string value)
        {
            value = value.ToLowerInvariant();

            // Unidecode for non-latin characters
             value = value.Unidecode(); 

            // Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            // Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            // Trim dashes & dots
            value = value.Trim('-', '_', '.');

            // Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
