using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Volo.CmsKit.Menus
{
    public static class EfCoreMenuExtensions
    {
        public static IQueryable<Menu> IncludeDetails(this IQueryable<Menu> source)
        {
            return source.Include(i => i.Items);
        }
    }
}