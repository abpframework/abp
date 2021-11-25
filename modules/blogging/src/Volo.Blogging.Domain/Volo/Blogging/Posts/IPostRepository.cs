using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Posts
{
    public interface IPostRepository : IBasicRepository<Post, Guid>
    {
        Task<List<Post>> GetPostsByBlogId(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsPostUrlInUseAsync(Guid blogId, string url, Guid? excludingPostId = null, CancellationToken cancellationToken = default);

        Task<Post> GetPostByUrl(Guid blogId, string url, CancellationToken cancellationToken = default);

        Task<List<Post>> GetOrderedList(Guid blogId,bool descending = false, CancellationToken cancellationToken = default);
    }
}
