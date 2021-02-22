using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public class EfCoreTransactionApi : ITransactionApi, ISupportsRollback
    {
        public IDbContextTransaction DbContextTransaction { get; }
        public IEfCoreDbContext StarterDbContext { get; }
        public List<IEfCoreDbContext> AttendedDbContexts { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public EfCoreTransactionApi(
            IDbContextTransaction dbContextTransaction,
            IEfCoreDbContext starterDbContext,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;
            CancellationTokenProvider = cancellationTokenProvider;
            AttendedDbContexts = new List<IEfCoreDbContext>();
        }

        public async Task CommitAsync()
        {
            await DbContextTransaction.CommitAsync(CancellationTokenProvider.Token);

            foreach (var dbContext in AttendedDbContexts)
            {
                if (dbContext.As<DbContext>().HasRelationalTransactionManager())
                {
                    continue; //Relational databases use the shared transaction
                }

                await dbContext.Database.CommitTransactionAsync(CancellationTokenProvider.Token);
            }
        }

        public void Dispose()
        {
            DbContextTransaction.Dispose();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken)
        {
            await DbContextTransaction.RollbackAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));

            foreach (var dbContext in AttendedDbContexts)
            {
                if (dbContext.As<DbContext>().HasRelationalTransactionManager())
                {
                    continue; //Relational databases use the shared transaction
                }

                await dbContext.Database.RollbackTransactionAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
            }
        }
    }
}
