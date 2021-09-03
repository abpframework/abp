using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Admin.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Blogs.ClientProxies
{
    public partial class BlogAdminClientProxy
    {
        public virtual async Task<BlogDto> GetAsync(Guid id)
        {
            return await RequestAsync<BlogDto>(nameof(GetAsync), id);
        }
 
        public virtual async Task<PagedResultDto<BlogDto>> GetListAsync(BlogGetListInput input)
        {
            return await RequestAsync<PagedResultDto<BlogDto>>(nameof(GetListAsync), input);
        }
 
        public virtual async Task<BlogDto> CreateAsync(CreateBlogDto input)
        {
            return await RequestAsync<BlogDto>(nameof(CreateAsync), input);
        }
 
        public virtual async Task<BlogDto> UpdateAsync(Guid id, UpdateBlogDto input)
        {
            return await RequestAsync<BlogDto>(nameof(UpdateAsync), id, input);
        }
 
        public virtual async Task DeleteAsync(Guid id)
        {
            await RequestAsync(nameof(DeleteAsync), id);
        }
 
    }
}
