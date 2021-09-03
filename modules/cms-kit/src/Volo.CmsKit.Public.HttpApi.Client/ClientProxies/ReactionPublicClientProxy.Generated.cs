using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Public.Reactions;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Reactions.ClientProxies
{
    public partial class ReactionPublicClientProxy
    {
        public virtual async Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(string entityType, string entityId)
        {
            return await RequestAsync<ListResultDto<ReactionWithSelectionDto>>(nameof(GetForSelectionAsync), entityType, entityId);
        }
 
        public virtual async Task CreateAsync(string entityType, string entityId, string reaction)
        {
            await RequestAsync(nameof(CreateAsync), entityType, entityId, reaction);
        }
 
        public virtual async Task DeleteAsync(string entityType, string entityId, string reaction)
        {
            await RequestAsync(nameof(DeleteAsync), entityType, entityId, reaction);
        }
 
    }
}
