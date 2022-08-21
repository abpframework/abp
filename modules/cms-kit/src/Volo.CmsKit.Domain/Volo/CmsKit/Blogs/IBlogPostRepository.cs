﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs;

public interface IBlogPostRepository : IBasicRepository<BlogPost, Guid>
{
    Task<int> GetCountAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        Guid? tagId = null,
        BlogPostStatus? statusFilter = null,
        CancellationToken cancellationToken = default);

    Task<List<BlogPost>> GetListAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        Guid? tagId = null,
        BlogPostStatus? statusFilter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default);

    Task<bool> SlugExistsAsync(Guid blogId, string slug, CancellationToken cancellationToken = default);

    Task<BlogPost> GetBySlugAsync(Guid blogId, string slug, CancellationToken cancellationToken = default);

    Task<List<CmsUser>> GetAuthorsHasBlogPostsAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string filter,
        CancellationToken cancellationToken = default);

    Task<int> GetAuthorsHasBlogPostsCountAsync(string filter, CancellationToken cancellationToken = default);

    Task<CmsUser> GetAuthorHasBlogPostAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> HasBlogPostWaitingForReviewAsync(CancellationToken cancellationToken = default);
}