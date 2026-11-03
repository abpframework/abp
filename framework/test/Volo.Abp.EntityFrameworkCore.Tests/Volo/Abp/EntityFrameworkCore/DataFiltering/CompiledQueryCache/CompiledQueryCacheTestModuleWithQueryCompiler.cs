using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class CompiledQueryCacheTestModuleWithQueryCompiler : CompiledQueryCacheTestModuleBase
{
    private readonly CompiledQueryCacheTestQueryCompilerOptions _queryCompilerOptions = new();

    protected override bool UseDbFunction => true;

    protected override bool UseExternalEfServiceProvider => false;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(_queryCompilerOptions);
        base.ConfigureServices(context);
    }

    protected override void ConfigureEfOptions(DbContextOptionsBuilder options)
    {
#pragma warning disable EF1001
        if (_queryCompilerOptions.ReplaceImplementationType)
        {
            options.ReplaceService<IQueryCompiler, QueryCompiler, CompiledQueryCacheTestQueryCompiler>();
        }
        else
        {
            options.ReplaceService<IQueryCompiler, CompiledQueryCacheTestQueryCompiler>();
        }
#pragma warning restore EF1001
    }
}

public class CompiledQueryCacheTestQueryCompilerOptions
{
    public bool ReplaceImplementationType { get; set; }
}
