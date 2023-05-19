using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Pages;

[Serializable]
public class PageSlugAlreadyExistsException : BusinessException
{
    public PageSlugAlreadyExistsException([NotNull] string slug)
    {
        Code = CmsKitErrorCodes.Pages.SlugAlreadyExist;
        WithData(nameof(Page.Slug), slug);
    }

    public PageSlugAlreadyExistsException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}
