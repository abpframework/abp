using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogPostManager
    {
        Task<BlogPost> CreateAsync(
            [NotNull] CmsUser author,
            [NotNull] Blog blog,
            [NotNull] string title,
            [NotNull] string slug,
            [CanBeNull] string shortDescription = null,
            [CanBeNull] Guid? tenantId = null);

        Task UpdateAsync(BlogPost blogPost);

        Task SetSlugUrlAsync(BlogPost blogPost, [NotNull] string newSlug);
    }
}
