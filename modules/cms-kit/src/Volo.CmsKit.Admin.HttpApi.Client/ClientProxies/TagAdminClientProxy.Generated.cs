using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Admin.Tags;
using Volo.CmsKit.Tags;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Tags.ClientProxies
{
    public partial class TagAdminClientProxy
    {
        public virtual async Task<TagDto> CreateAsync(TagCreateDto input)
        {
            return await RequestAsync<TagDto>(nameof(CreateAsync), input);
        }
 
        public virtual async Task DeleteAsync(Guid id)
        {
            await RequestAsync(nameof(DeleteAsync), id);
        }
 
        public virtual async Task<TagDto> GetAsync(Guid id)
        {
            return await RequestAsync<TagDto>(nameof(GetAsync), id);
        }
 
        public virtual async Task<PagedResultDto<TagDto>> GetListAsync(TagGetListInput input)
        {
            return await RequestAsync<PagedResultDto<TagDto>>(nameof(GetListAsync), input);
        }
 
        public virtual async Task<TagDto> UpdateAsync(Guid id, TagUpdateDto input)
        {
            return await RequestAsync<TagDto>(nameof(UpdateAsync), id, input);
        }
 
        public virtual async Task<List<TagDefinitionDto>> GetTagDefinitionsAsync()
        {
            return await RequestAsync<List<TagDefinitionDto>>(nameof(GetTagDefinitionsAsync));
        }
 
    }
}
