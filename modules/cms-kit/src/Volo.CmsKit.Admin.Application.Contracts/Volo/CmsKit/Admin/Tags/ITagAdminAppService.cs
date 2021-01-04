using System;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
    public interface ITagAdminAppService : ICrudAppService<TagDto, Guid, TagGetListInput, TagCreateDto, TagUpdateDto>
    {
    }
}
