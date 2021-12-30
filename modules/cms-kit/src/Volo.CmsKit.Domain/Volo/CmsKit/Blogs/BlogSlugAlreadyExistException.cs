using System;
using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.CmsKit.Blogs;

[Serializable]
public class BlogSlugAlreadyExistException : BusinessException
{
    public BlogSlugAlreadyExistException(string slug)
        : base(code: CmsKitErrorCodes.Blogs.SlugAlreadyExists)
    {
        WithData(nameof(Blog.Slug), slug);
    }

    public BlogSlugAlreadyExistException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }
}
