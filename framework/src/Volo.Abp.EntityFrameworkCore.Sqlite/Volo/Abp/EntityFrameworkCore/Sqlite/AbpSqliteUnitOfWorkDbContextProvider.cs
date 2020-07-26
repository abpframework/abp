using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Volo.Abp.Uow.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore.Sqlite
{
    public class AbpSqliteUnitOfWorkDbContextProvider<TDbContext> : UnitOfWorkDbContextProvider<TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        public AbpSqliteUnitOfWorkDbContextProvider(IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver,
            IOptions<AbpEntityFrameworkCoreSqliteOptions> optionsAccessor)
            : base(unitOfWorkManager, connectionStringResolver)
        {
            Options = optionsAccessor.Value;
        }

        protected AbpEntityFrameworkCoreSqliteOptions Options { get; }

        protected override TDbContext CreateDbContext(IUnitOfWork unitOfWork)
        {
            if (Options.DisableTransaction)
            {
                return unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
            }

            return base.CreateDbContext(unitOfWork);
        }
    }
}
