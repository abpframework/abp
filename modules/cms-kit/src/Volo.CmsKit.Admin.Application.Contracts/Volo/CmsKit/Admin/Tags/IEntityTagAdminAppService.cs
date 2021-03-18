using System.Threading.Tasks;

namespace Volo.CmsKit.Admin.Tags
{
    public interface IEntityTagAdminAppService
    {
        Task AddTagToEntityAsync(EntityTagCreateDto input);

        Task RemoveTagFromEntityAsync(EntityTagRemoveDto input);

        Task SetEntityTagsAsync(EntityTagSetDto input);
    }
}
