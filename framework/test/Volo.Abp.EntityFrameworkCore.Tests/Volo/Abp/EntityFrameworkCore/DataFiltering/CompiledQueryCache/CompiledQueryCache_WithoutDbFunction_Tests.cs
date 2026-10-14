namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_WithoutDbFunction_Tests : CompiledQueryCacheTestBase<CompiledQueryCacheTestModuleWithoutDbFunction>
{
    protected override bool UseDbFunction => false;
}
