using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Tags;

public interface IEntityTagAdminAppService : IApplicationService
{
    Task AddTagToEntityAsync(EntityTagCreateDto input);

    Task RemoveTagFromEntityAsync(EntityTagRemoveDto input);

    Task SetEntityTagsAsync(EntityTagSetDto input);
}
