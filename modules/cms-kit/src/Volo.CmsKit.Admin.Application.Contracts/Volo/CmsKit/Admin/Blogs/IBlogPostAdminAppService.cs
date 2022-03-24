using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Blogs;

public interface IBlogPostAdminAppService
    : ICrudAppService<
        BlogPostDto,
        BlogPostListDto,
        Guid,
        BlogPostGetListInput,
        CreateBlogPostDto,
        UpdateBlogPostDto>
{
    Task PublishAsync(Guid id);
    
    Task DraftAsync(Guid id);
    
    Task<BlogPostDto> CreateAndPublishAsync(CreateBlogPostDto input);
}
