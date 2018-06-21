using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Blogging.EntityFrameworkCore
{
    [ConnectionStringName("Blogging")]
    public interface IBloggingDbContext : IEfCoreDbContext
    {
        
    }
}