using System;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Admin.Contents;

namespace Volo.CmsKit.Admin.Contents
{
    public interface IContentAdminAppService 
        : ICrudAppService<
            ContentDto,
            Guid,
            ContentGetListInput,
            ContentCreateDto,
            ContentUpdateDto>
    {
    }
}
