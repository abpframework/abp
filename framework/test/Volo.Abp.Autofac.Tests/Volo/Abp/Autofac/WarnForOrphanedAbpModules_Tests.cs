using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Volo.Abp.Logging;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Autofac;

public class WarnForOrphanedAbpModules_Tests : AbpIntegratedTest<WarnForOrphanedAbpModules_Tests.TestModule>
{
    private static readonly Type OrphanModuleType = typeof(AbpTestModule);

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public void Should_Warn_For_Orphaned_Abp_Modules()
    {
        var initLoggerFactory = GetRequiredService<IInitLoggerFactory>();
        var logger = initLoggerFactory.Create<AbpAutofacServiceProviderFactory>();

        logger.Entries
            .Where(e => e.LogLevel == LogLevel.Warning)
            .ShouldContain(e => e.Message.Contains(OrphanModuleType.FullName!),
                $"Expected a warning for orphaned module '{OrphanModuleType.FullName}'.");
    }

    [Fact]
    public void Should_Not_Warn_For_Loaded_Modules()
    {
        var initLoggerFactory = GetRequiredService<IInitLoggerFactory>();
        var logger = initLoggerFactory.Create<AbpAutofacServiceProviderFactory>();

        // AbpAutofacModule IS in the DependsOn chain, so it should NOT trigger a warning.
        logger.Entries
            .Where(e => e.LogLevel == LogLevel.Warning)
            .ShouldNotContain(e => e.Message.Contains(typeof(AbpAutofacModule).FullName!),
                "Modules in the [DependsOn] chain should not be reported as orphaned.");
    }

    [DependsOn(typeof(AbpAutofacModule))]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Simulate ASP.NET Core's AddControllersAsServices() registering a type
            // from an assembly whose ABP module is NOT in the [DependsOn] chain.
            // AbpTestModule lives in the Volo.Abp.Core.Tests assembly which is not depended on.
            context.Services.AddTransient<AbpTestModule>();
        }
    }
}
