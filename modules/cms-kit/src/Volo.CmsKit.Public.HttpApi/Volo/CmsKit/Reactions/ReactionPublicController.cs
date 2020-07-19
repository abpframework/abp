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
        [Route("{entityType}/{entityId}")]
        public virtual Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(string entityType, string entityId)
        {
            return ReactionPublicAppService.GetForSelectionAsync(entityType, entityId);
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
