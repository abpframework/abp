using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public class EfCoreTransactionApi : IEfCoreTransactionApi
    {
        public IDbContextTransaction DbContextTransaction { get; }
        public DbContext StarterDbContext { get; }
        public List<DbContext> AttendedDbContexts { get; }

        public EfCoreTransactionApi(IDbContextTransaction dbContextTransaction, DbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;
            AttendedDbContexts = new List<DbContext>();
        }

        public void Commit()
        {
            DbContextTransaction.Commit();

            foreach (var dbContext in AttendedDbContexts)
            {
                if (dbContext.HasRelationalTransactionManager())
                {
                    continue; //Relational databases use the shared transaction
                }

                dbContext.Database.CommitTransaction();
            }
        }

        public Task CommitAsync()
        {
            Commit();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            DbContextTransaction.Dispose();
        }
    }
}