using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags
{
    public interface ITagAdminAppService : ICrudAppService<TagDto, Guid, TagGetListInput, TagCreateDto, TagUpdateDto>
    {
        Task<List<TagDefinitionDto>> GetTagDefinitionsAsync();
    }
}
