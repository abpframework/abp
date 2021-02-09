using Slugify;
using System.Text.RegularExpressions;
using Unidecode.NET;

namespace Volo.CmsKit.Blogs.Extensions
{
    public static class UrlSlugExtensions
    {
        public static string NormalizeAsUrlSlug(this string value)
        {
            var slugHelper = new SlugHelper();

            return slugHelper.GenerateSlug(value);
        }
    }
}
