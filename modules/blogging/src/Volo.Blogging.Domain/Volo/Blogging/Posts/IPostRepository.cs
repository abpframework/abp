﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Posts
{
    public interface IPostRepository : IBasicRepository<Post, Guid>
    {
        Task<List<Post>> GetPostsByBlogId(Guid id);

        Task<Post> GetPostByUrl(Guid blogId, string url);
        
        Task<List<Post>> GetOrderedList(Guid blogId,bool descending = false);
    }
}
