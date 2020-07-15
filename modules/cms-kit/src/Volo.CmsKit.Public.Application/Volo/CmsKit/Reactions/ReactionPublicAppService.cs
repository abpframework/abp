using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Reactions
{
    public class ReactionPublicAppService : ApplicationService, IReactionPublicAppService
    {
        public Task<ListResultDto<ReactionDto>> GetAvailableReactions(
            GetAvailableReactionsDto getAvailableReactionsDto)
        {
            return Task.FromResult(
                new ListResultDto<ReactionDto>(new List<ReactionDto>()
                {
                    new ReactionDto {Name = StandardReactions.ThumbsUp},
                    new ReactionDto {Name = StandardReactions.Smile, DisplayName = "Smile :)"}
                })
            );
        }
    }
}
