using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public interface IEfCoreTransactionStrategy
    {
        TDbContext CreateDbContext<TDbContext>(IUnitOfWork unitOfWork, DbContextCreationContext creationContext)
            where TDbContext : DbContext;
    }
}
