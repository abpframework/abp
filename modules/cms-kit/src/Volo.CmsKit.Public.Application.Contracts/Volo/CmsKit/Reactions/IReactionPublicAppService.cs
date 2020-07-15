using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Reactions
{
    public interface IReactionPublicAppService : IApplicationService
    {
        Task<ListResultDto<ReactionDto>> GetAvailableReactions(
            GetAvailableReactionsDto input
        );

        Task<ListResultDto<ReactionSummaryDto>> GetReactionSummariesAsync(
            GetReactionSummariesDto input
        );

        Task<ListResultDto<ReactionDto>> GetMyReactions(
            GetMyReactionsDto input
        );

        Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(GetForSelectionDto input);

        Task CreateAsync(CreateReactionDto input);

        Task DeleteAsync(DeleteReactionDto input);
    }
}
