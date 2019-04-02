using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace BaseManagement.EntityFrameworkCore
{
    [ConnectionStringName("BaseManagement")]
    public interface IBaseManagementDbContext : IEfCoreDbContext
    {
         DbSet<BaseType> BaseTypes { get; }
         DbSet<BaseItem> BaseItems { get; }
    }
}