using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Pages;

public class PageSlugAlreadyExistsException : BusinessException
{
    public PageSlugAlreadyExistsException([NotNull] string slug)
    {
        Code = CmsKitErrorCodes.Pages.SlugAlreadyExist;
        WithData(nameof(Page.Slug), slug);
    }
}
