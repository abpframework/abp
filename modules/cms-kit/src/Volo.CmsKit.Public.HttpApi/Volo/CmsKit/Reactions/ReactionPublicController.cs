using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Reactions
{
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/reactions")]
    public class ReactionPublicController : CmsKitPublicControllerBase, IReactionPublicAppService
    {
        protected IReactionPublicAppService ReactionPublicAppService { get; }

        public ReactionPublicController(IReactionPublicAppService reactionPublicAppService)
        {
            ReactionPublicAppService = reactionPublicAppService;
        }

        [HttpGet]
        [Route("available")]
        public virtual Task<ListResultDto<ReactionDto>> GetAvailableReactions(GetAvailableReactionsDto input)
        {
            return ReactionPublicAppService.GetAvailableReactions(input);
        }

        [HttpGet]
        [Route("summaries")]
        public virtual Task<ListResultDto<ReactionSummaryDto>> GetReactionSummariesAsync(GetReactionSummariesDto input)
        {
            return ReactionPublicAppService.GetReactionSummariesAsync(input);
        }

        [HttpGet]
        [Route("my")]
        public virtual  Task<ListResultDto<ReactionDto>> GetMyReactions(GetMyReactionsDto input)
        {
            return ReactionPublicAppService.GetMyReactions(input);
        }

        [HttpGet]
        [Route("selection")] //TODO: Consider to rename!
        public virtual Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(GetForSelectionDto input)
        {
            return ReactionPublicAppService.GetForSelectionAsync(input);
        }

        [HttpPut]
        public virtual Task CreateAsync(CreateReactionDto input)
        {
            return ReactionPublicAppService.CreateAsync(input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync(DeleteReactionDto input)
        {
            return ReactionPublicAppService.DeleteAsync(input);
        }
    }
}
