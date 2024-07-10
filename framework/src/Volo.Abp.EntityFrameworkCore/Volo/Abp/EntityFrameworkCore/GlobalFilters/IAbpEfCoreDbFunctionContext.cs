using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public interface IAbpEfCoreDbFunctionContext
{
    IAbpLazyServiceProvider LazyServiceProvider { get; set; }

    ICurrentTenant CurrentTenant { get; }

    IDataFilter DataFilter { get; }

    string GetCompiledQueryCacheKey();
}
