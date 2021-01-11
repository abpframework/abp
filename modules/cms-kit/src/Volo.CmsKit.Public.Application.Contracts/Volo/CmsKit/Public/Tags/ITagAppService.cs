using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Public.Tags
{
    public interface ITagAppService : IApplicationService
    {
        Task<List<TagDto>> GetAllRelatedTagsAsync(string entityType, string entityId);
    }
}