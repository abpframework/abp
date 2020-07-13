using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Reactions
{
    public interface IReactionAppService : IApplicationService
    {
        public Task<GetReactionResultDto> CreateAsync(CreateReactionDto input);

        public Task<GetReactionResultDto> GetAsync(string entityName, string entityId);
    }
}
