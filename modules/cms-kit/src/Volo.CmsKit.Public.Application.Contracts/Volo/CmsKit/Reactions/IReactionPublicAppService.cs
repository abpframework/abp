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
    }
}
