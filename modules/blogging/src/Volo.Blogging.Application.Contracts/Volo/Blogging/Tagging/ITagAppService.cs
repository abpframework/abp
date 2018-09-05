using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Tagging
{
    public interface ITagAppService : IApplicationService
    {
        Task<List<TagDto>> GetListAsync();

        Task<List<TagDto>> GetListAsync(GetTagListInput input);

        Task<TagDto> CreateAsync(CreateTagDto input);

        Task<TagDto> UpdateAsync(Guid id, UpdateTagDto input);

        Task DeleteAsync(Guid id);

        Task<List<TagDto>> GetPopularTags(GetPopularTagsInput input);

    }
}
