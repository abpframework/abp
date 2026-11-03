using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.FeatureManagement;

public class AbpFeatureManagementDomainModule_Shutdown_Tests
{
    [Fact]
    public async Task Shutdown_Should_Cancel_The_Dynamic_Initialization()
    {
        using var application = await AbpApplicationFactory.CreateAsync<ShutdownTestModule>(options => options.UseAutofac());
        await application.InitializeAsync();
        var token = application.ServiceProvider.GetRequiredService<InitializationTokenRecorder>().Token;
        token.IsCancellationRequested.ShouldBeFalse();

        application.Shutdown();

        token.IsCancellationRequested.ShouldBeTrue();
    }

    [Fact]
    public async Task ShutdownAsync_Should_Cancel_The_Dynamic_Initialization()
    {
        using var application = await AbpApplicationFactory.CreateAsync<ShutdownTestModule>(options => options.UseAutofac());
        await application.InitializeAsync();
        var token = application.ServiceProvider.GetRequiredService<InitializationTokenRecorder>().Token;

        await application.ShutdownAsync();

        token.IsCancellationRequested.ShouldBeTrue();
    }

    public class InitializationTokenRecorder
    {
        public CancellationToken Token { get; set; }
    }

    [DisableConventionalRegistration]
    public class RecordingFeatureDynamicInitializer : FeatureDynamicInitializer
    {
        public RecordingFeatureDynamicInitializer(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override Task InitializeAsync(bool runInBackground, CancellationToken cancellationToken = default)
        {
            // The module starts it in the background; a test module may run it again in the foreground without a token.
            if (runInBackground)
            {
                ServiceProvider.GetRequiredService<InitializationTokenRecorder>().Token = cancellationToken;
            }

            return Task.CompletedTask;
        }
    }

    [DependsOn(typeof(AbpFeatureManagementDomainTestModule))]
    public class ShutdownTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<InitializationTokenRecorder>();
            context.Services.Replace(ServiceDescriptor.Transient<FeatureDynamicInitializer, RecordingFeatureDynamicInitializer>());
        }
    }
}
