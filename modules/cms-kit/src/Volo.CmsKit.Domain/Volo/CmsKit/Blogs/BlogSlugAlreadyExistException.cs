using Volo.Abp;

namespace Volo.CmsKit.Blogs;

public class BlogSlugAlreadyExistException : BusinessException
{
    public BlogSlugAlreadyExistException(string slug)
        : base(code: CmsKitErrorCodes.Blogs.SlugAlreadyExists)
    {
        WithData(nameof(Blog.Slug), slug);
    }
}
