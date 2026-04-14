using System;
using System.Collections.Generic;
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
    private List<AbpInitLogEntry> _initLogEntries = [];

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    protected override IServiceProvider CreateServiceProvider(IServiceCollection services)
    {
        var serviceProvider = base.CreateServiceProvider(services);

        // Capture init log entries after Autofac Populate/Register but before WriteInitLogs clears them.
        _initLogEntries = serviceProvider.GetRequiredService<IInitLoggerFactory>().GetAllEntries();

        return serviceProvider;
    }

    [Fact]
    public void Should_Warn_For_Orphaned_Abp_Modules()
    {
        _initLogEntries
            .Where(e => e.LogLevel == LogLevel.Warning)
            .ShouldContain(e => e.Message.Contains(OrphanModuleType.FullName!),
                $"Expected a warning for orphaned module '{OrphanModuleType.FullName}'.");
    }

    [Fact]
    public void Should_Not_Warn_For_Loaded_Modules()
    {
        _initLogEntries
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
            context.Services.AddTransient<AbpTestModule>();
        }
    }
}
