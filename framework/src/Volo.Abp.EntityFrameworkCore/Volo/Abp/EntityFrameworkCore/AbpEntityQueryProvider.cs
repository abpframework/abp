using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;

namespace Volo.Abp.EntityFrameworkCore;

#pragma warning disable EF1001
public class AbpEntityQueryProvider : EntityQueryProvider
{
    protected AbpEfCoreCurrentDbContext AbpEfCoreCurrentDbContext { get; }
    protected ICurrentDbContext CurrentDbContext { get; }

    private bool _queryServicesChecked;

    public AbpEntityQueryProvider(
        IQueryCompiler queryCompiler,
        AbpEfCoreCurrentDbContext abpEfCoreCurrentDbContext,
        ICurrentDbContext currentDbContext)
        : base(queryCompiler)
    {
        AbpEfCoreCurrentDbContext = abpEfCoreCurrentDbContext;
        CurrentDbContext = currentDbContext;
    }

    public override object Execute(Expression expression)
    {
        CheckQueryServices();
        using (AbpEfCoreCurrentDbContext.BeginQuery(CurrentDbContext.Context as IAbpEfCoreDbFunctionContext))
        {
            return base.Execute(expression);
        }
    }

    public override TResult Execute<TResult>(Expression expression)
    {
        CheckQueryServices();
        using (AbpEfCoreCurrentDbContext.BeginQuery(CurrentDbContext.Context as IAbpEfCoreDbFunctionContext))
        {
            return base.Execute<TResult>(expression);
        }
    }

    public override TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
    {
        CheckQueryServices();
        using (AbpEfCoreCurrentDbContext.BeginQuery(CurrentDbContext.Context as IAbpEfCoreDbFunctionContext))
        {
            return base.ExecuteAsync<TResult>(expression, cancellationToken);
        }
    }

    protected virtual void CheckQueryServices()
    {
        if (_queryServicesChecked)
        {
            return;
        }

        var dbContext = CurrentDbContext.Context;
        if (dbContext is IAbpEfCoreDbFunctionContext abpDbContext && abpDbContext.UseDbFunction() && dbContext.Database.IsRelational() &&
            (dbContext.GetService<IDatabase>() is not AbpEfCoreDatabase ||
             dbContext.GetService<ICompiledQueryCacheKeyGenerator>() is not AbpCompiledQueryCacheKeyGenerator))
        {
            throw new AbpException(
                "The data filters cannot be applied, because the EF Core services of ABP are not registered. " +
                "Add AbpDbContextOptionsExtension after the database provider, or set 'UseDbFunction' of 'AbpEfCoreGlobalFilterOptions' to false.");
        }

        _queryServicesChecked = true;
    }
}
#pragma warning restore EF1001
