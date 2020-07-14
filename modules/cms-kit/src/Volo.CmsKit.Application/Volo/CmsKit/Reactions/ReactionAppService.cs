using System.Threading.Tasks;

namespace Volo.CmsKit.Reactions
{
    public class ReactionAppService : CmsKitAppService, IReactionAppService
    {
        public Task<GetReactionResultDto> CreateAsync(CreateReactionDto input)
        {
            throw new System.NotImplementedException();
        }

        public Task<GetReactionResultDto> GetAsync(string entityName, string entityId)
        {
            throw new System.NotImplementedException();
        }
    }
}
