using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public class EfCoreDatabaseApi : IDatabaseApi, ISupportsSavingChanges
    {
        public IEfCoreDbContext DbContext { get; }

        public EfCoreDatabaseApi(IEfCoreDbContext dbContext)
        {
            DbContext = dbContext;
        }
        
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}