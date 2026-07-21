namespace Volo.Abp.EntityFrameworkCore.GlobalFilters;

public interface IAbpEfCoreCompiledQueryCacheKeyProvider
{
    string? GetCompiledQueryCacheKey();
}
