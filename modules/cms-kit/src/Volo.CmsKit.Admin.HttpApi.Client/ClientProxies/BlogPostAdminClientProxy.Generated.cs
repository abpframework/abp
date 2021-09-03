using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Modeling;
using Volo.CmsKit.Admin.Blogs;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.Blogs.ClientProxies
{
    public partial class BlogPostAdminClientProxy
    {
        public virtual async Task<BlogPostDto> CreateAsync(CreateBlogPostDto input)
        {
            return await RequestAsync<BlogPostDto>(nameof(CreateAsync), input);
        }
 
        public virtual async Task DeleteAsync(Guid id)
        {
            await RequestAsync(nameof(DeleteAsync), id);
        }
 
        public virtual async Task<BlogPostDto> GetAsync(Guid id)
        {
            return await RequestAsync<BlogPostDto>(nameof(GetAsync), id);
        }
 
        public virtual async Task<PagedResultDto<BlogPostListDto>> GetListAsync(BlogPostGetListInput input)
        {
            return await RequestAsync<PagedResultDto<BlogPostListDto>>(nameof(GetListAsync), input);
        }
 
        public virtual async Task<BlogPostDto> UpdateAsync(Guid id, UpdateBlogPostDto input)
        {
            return await RequestAsync<BlogPostDto>(nameof(UpdateAsync), id, input);
        }
 
    }
}
