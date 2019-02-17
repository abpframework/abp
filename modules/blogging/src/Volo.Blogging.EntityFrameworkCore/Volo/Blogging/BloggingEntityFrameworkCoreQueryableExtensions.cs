using Volo.Blogging.Posts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Volo.Blogging
{
    public static class BloggingEntityFrameworkCoreQueryableExtensions
    {
        public static IQueryable<Post> IncludeDetails(this IQueryable<Post> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                .Include(x => x.Tags);
        }
    }
}
