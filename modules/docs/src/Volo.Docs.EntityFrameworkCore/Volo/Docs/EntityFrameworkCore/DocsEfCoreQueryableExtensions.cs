using System.Linq;
using Microsoft.EntityFrameworkCore;
using Volo.Docs.Documents;

namespace Volo.Docs.EntityFrameworkCore
{
    public static class DocsEfCoreQueryableExtensions
    {
        public static IQueryable<Document> IncludeDetails(this IQueryable<Document> queryable, bool include = true)
        {
            return !include ? queryable : queryable.Include(x => x.Contributors);
        }
    }
}