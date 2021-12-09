using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories.Dapper;

public class DapperRepository<TDbContext> : IDapperRepository, IUnitOfWorkEnabled
    where TDbContext : IEfCoreDbContext
{
    private readonly IDbContextProvider<TDbContext> _dbContextProvider;

    public DapperRepository(IDbContextProvider<TDbContext> dbContextProvider)
    {
        _dbContextProvider = dbContextProvider;
    }

    [Obsolete("Use GetDbConnectionAsync method.")]
    public IDbConnection DbConnection => _dbContextProvider.GetDbContext().Database.GetDbConnection();

    public async Task<IDbConnection> GetDbConnectionAsync() => (await _dbContextProvider.GetDbContextAsync()).Database.GetDbConnection();

    [Obsolete("Use GetDbTransactionAsync method.")]
    public IDbTransaction DbTransaction => _dbContextProvider.GetDbContext().Database.CurrentTransaction?.GetDbTransaction();

    public async Task<IDbTransaction> GetDbTransactionAsync() => (await _dbContextProvider.GetDbContextAsync()).Database.CurrentTransaction?.GetDbTransaction();
}
