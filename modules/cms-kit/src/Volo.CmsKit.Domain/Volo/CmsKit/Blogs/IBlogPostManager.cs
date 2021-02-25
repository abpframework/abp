﻿using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogPostManager
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);

        Task UpdateAsync(BlogPost blogPost);

        Task SetSlugUrlAsync(BlogPost blogPost, [NotNull] string newSlug);
    }
}
