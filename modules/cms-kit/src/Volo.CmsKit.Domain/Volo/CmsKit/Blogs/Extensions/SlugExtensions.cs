using Slugify;
using Unidecode.NET;

namespace Volo.CmsKit.Blogs.Extensions
{
    public static class SlugExtensions
    {
        public static string NormalizeSlug(this string value)
        {
            var slugHelper = new SlugHelper();

            return slugHelper.GenerateSlug(value?.Unidecode());
        }
    }
}
