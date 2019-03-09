using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace ProductManagement.EntityFrameworkCore
{
    [ConnectionStringName("ProductManagement")]
    public interface IProductManagementDbContext : IEfCoreDbContext
    {
         DbSet<Product> Products { get; }
    }
}