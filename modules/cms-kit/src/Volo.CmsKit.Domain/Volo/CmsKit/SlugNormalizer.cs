using Slugify;
using Unidecode.NET;

namespace Volo.CmsKit;

public static class SlugNormalizer
{
    static readonly SlugHelper SlugHelper = new(new SlugHelperConfiguration
    {
        AllowedCharacters =
        {
            '/'
        }
    });

    public static string Normalize(string value)
    {
        return SlugHelper.GenerateSlug(value?.Unidecode()).Trim('/');
    }
}
