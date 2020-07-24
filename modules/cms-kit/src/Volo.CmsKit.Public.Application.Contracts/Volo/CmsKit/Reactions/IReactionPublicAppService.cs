using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Reactions
{
    public interface IReactionPublicAppService : IApplicationService
    {
        Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(string entityType, string entityId);

        Task CreateAsync(CreateReactionDto input);

        Task DeleteAsync(DeleteReactionDto input);
    }
}
