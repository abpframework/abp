using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    public interface IEfCoreDbContext
    {
        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        DbSet<T> Set<T>()
            where T: class;
    }
}
