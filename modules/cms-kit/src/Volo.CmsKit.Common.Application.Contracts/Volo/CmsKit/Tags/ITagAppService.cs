using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Tags
{
    public interface ITagAppService : IApplicationService
    {
        Task<List<TagDto>> GetAllRelatedTagsAsync(GetRelatedTagsInput input);
    }
}
