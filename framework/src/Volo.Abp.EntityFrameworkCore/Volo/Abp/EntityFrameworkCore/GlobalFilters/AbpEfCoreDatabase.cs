using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public class AbpEfCoreDatabase : IDatabase
{
    protected IDatabase InnerDatabase { get; }
    protected AbpEfCoreCurrentDbContext AbpEfCoreCurrentDbContext { get; }
    protected ICurrentDbContext CurrentDbContext { get; }

    public AbpEfCoreDatabase(
        IDatabase innerDatabase,
        AbpEfCoreCurrentDbContext abpEfCoreCurrentDbContext,
        ICurrentDbContext currentDbContext)
    {
        InnerDatabase = innerDatabase;
        AbpEfCoreCurrentDbContext = abpEfCoreCurrentDbContext;
        CurrentDbContext = currentDbContext;
    }

    public virtual int SaveChanges(IList<IUpdateEntry> entries)
    {
        return InnerDatabase.SaveChanges(entries);
    }

    public virtual Task<int> SaveChangesAsync(IList<IUpdateEntry> entries, CancellationToken cancellationToken = default)
    {
        return InnerDatabase.SaveChangesAsync(entries, cancellationToken);
    }

    public virtual Func<QueryContext, TResult> CompileQuery<TResult>(Expression query, bool async)
    {
        using (AbpEfCoreCurrentDbContext.BeginQueryCompilation(CurrentDbContext.Context as IAbpEfCoreDbFunctionContext, query))
        {
            return InnerDatabase.CompileQuery<TResult>(query, async);
        }
    }

#pragma warning disable EF9100
    public virtual Expression<Func<QueryContext, TResult>> CompileQueryExpression<TResult>(Expression query, bool async)
    {
        using (AbpEfCoreCurrentDbContext.BeginQueryCompilation(CurrentDbContext.Context as IAbpEfCoreDbFunctionContext, query))
        {
            return InnerDatabase.CompileQueryExpression<TResult>(query, async);
        }
    }
#pragma warning restore EF9100
}
