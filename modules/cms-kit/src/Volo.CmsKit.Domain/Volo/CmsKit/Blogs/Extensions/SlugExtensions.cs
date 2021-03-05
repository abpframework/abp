using Slugify;
using Unidecode.NET;

namespace Volo.CmsKit.Blogs.Extensions
{
    public static class SlugExtensions
    {
        static readonly SlugHelper SlugHelper = new ();
        public static string NormalizeSlug(this string value) //TODO: Should not be an extension method.
        {
            return SlugHelper.GenerateSlug(value?.Unidecode());
        }
    }
}
