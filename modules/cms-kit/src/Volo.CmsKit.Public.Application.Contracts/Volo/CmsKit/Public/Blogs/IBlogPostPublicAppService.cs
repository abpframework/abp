using System.Collections.Generic;
using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.CmsKit.Users;
using System;

namespace Volo.CmsKit.Public.Blogs;

public interface IBlogPostPublicAppService : IApplicationService
{
    Task<PagedResultDto<BlogPostPublicDto>> GetListAsync([NotNull] string blogSlug, BlogPostGetListInput input);

    Task<BlogPostPublicDto> GetAsync([NotNull] string blogSlug, [NotNull] string blogPostSlug);

    Task<PagedResultDto<CmsUserDto>> GetAuthorsHasBlogPostsAsync(BlogPostFilteredPagedAndSortedResultRequestDto input);

    Task<CmsUserDto> GetAuthorHasBlogPostAsync(Guid id);
}
