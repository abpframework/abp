using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Volo.Abp.Uow.EntityFrameworkCore
{
    public class DbContextEfCoreTransactionStrategy : IEfCoreTransactionStrategy, ITransientDependency
    {
        public TDbContext CreateDbContext<TDbContext>(IUnitOfWork unitOfWork, DbContextCreationContext creationContext) where TDbContext : DbContext
        {
            var transactionApiKey = $"EntityFrameworkCore_{creationContext.ConnectionString}";
            var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as IEfCoreTransactionApi;

            TDbContext dbContext;
            if (activeTransaction == null)
            {
                dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
                var dbtransaction = dbContext.Database.BeginTransaction((unitOfWork.Options.IsolationLevel ?? IsolationLevel.ReadUncommitted));
                activeTransaction = new EfCoreTransactionApi(dbtransaction, dbContext);
                unitOfWork.AddTransactionApi(transactionApiKey, activeTransaction);
            }
            else
            {
                creationContext.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;
                dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
                
                if (dbContext.HasRelationalTransactionManager())
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction();
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);
            }

            return dbContext;
        }
    }
}