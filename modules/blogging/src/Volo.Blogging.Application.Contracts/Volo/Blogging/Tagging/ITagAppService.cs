using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Blogging.Tagging.Dtos;

namespace Volo.Blogging.Tagging
{
    public interface ITagAppService : IApplicationService
    {
        Task<List<TagDto>> GetPopularTagsAsync(Guid blogId, GetPopularTagsInput input);

    }
}
