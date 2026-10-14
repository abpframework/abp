namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_Tests : CompiledQueryCacheTestBase<CompiledQueryCacheTestModule>
{
    protected override bool UseDbFunction => true;
}
