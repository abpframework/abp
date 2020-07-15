using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

        Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(GetForSelectionInput input);
    }
}
