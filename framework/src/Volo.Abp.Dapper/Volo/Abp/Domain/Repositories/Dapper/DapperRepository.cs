using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories.Dapper
{
    public class DapperRepository<TDbContext> : IDapperRepository, IUnitOfWorkEnabled
        where TDbContext : IEfCoreDbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public DapperRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public IDbConnection DbConnection => _dbContextProvider.GetDbContext().Database.GetDbConnection();

        public IDbTransaction DbTransaction => _dbContextProvider.GetDbContext().Database.CurrentTransaction?.GetDbTransaction();
    }
}