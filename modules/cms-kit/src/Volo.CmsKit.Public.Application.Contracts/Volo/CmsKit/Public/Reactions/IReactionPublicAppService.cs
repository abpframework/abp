using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Reactions
{
    public interface IReactionPublicAppService : IApplicationService
    {
        Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(string entityType, string entityId);

        Task CreateAsync(string entityType, string entityId, string reaction);

        Task DeleteAsync(string entityType, string entityId, string reaction);
    }
}
