using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Blog.EntityFrameworkCore
{
    [ConnectionStringName("Blog")]
    public interface IBlogDbContext : IEfCoreDbContext
    {
        
    }
}