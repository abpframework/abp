using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Shouldly;
using Volo.Abp.Autofac;
using Volo.Abp.Internal.Telemetry;
using Volo.Abp.Internal.Telemetry.Activity;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AspNetCore;

public class TelemetryApplicationLifetime_Tests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Should_Release_Applications_And_Autofac_Containers_After_Telemetry_Completes(bool useIntegratedTest)
    {
        var references = new List<ApplicationReferences>();
        for (var i = 0; i < 10; i++)
        {
            references.Add(CreateAndDisposeApplication(useIntegratedTest));
        }

        var collected = SpinWait.SpinUntil(() =>
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return references.All(reference => !reference.Application.IsAlive && !reference.Container.IsAlive);
        }, TimeSpan.FromSeconds(10));

        collected.ShouldBeTrue("Disposed applications and their root Autofac containers must be collectible.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static ApplicationReferences CreateAndDisposeApplication(bool useIntegratedTest)
    {
        if (useIntegratedTest)
        {
            using var test = new TelemetryIntegratedTest();
            return test.GetReferences();
        }

        using var application = AbpApplicationFactory.Create<TelemetryTestModule>(options => options.UseAutofac());
        application.Initialize();
        try
        {
            return ObserveApplication(application, application.ServiceProvider);
        }
        finally
        {
            application.Shutdown();
        }
    }

    private static ApplicationReferences ObserveApplication(IAbpApplication application, IServiceProvider serviceProvider)
    {
        serviceProvider.GetService<IWebHostEnvironment>().ShouldBeNull();
        serviceProvider.GetRequiredService<IAbpHostEnvironment>().EnvironmentName.ShouldBe(Environments.Development);
        serviceProvider.GetRequiredService<IConfiguration>().GetValue<bool?>("Abp:Telemetry:IsEnabled").ShouldBeNull();

        var completion = serviceProvider.GetRequiredService<TelemetryCompletion>();
        completion.ScopeDisposed.Task.Wait(TimeSpan.FromSeconds(10)).ShouldBeTrue();
        completion.ActivityCount.ShouldBe(1);
        completion.ActivityName.ShouldBe(ActivityNameConsts.ApplicationRun);

        var lifetimeScope = (ISharingLifetimeScope)serviceProvider.GetRequiredService<ILifetimeScope>();
        return new ApplicationReferences(new WeakReference(application), new WeakReference(lifetimeScope.RootLifetimeScope));
    }

    private sealed record ApplicationReferences(WeakReference Application, WeakReference Container);

    private sealed class TelemetryIntegratedTest : AbpIntegratedTest<TelemetryTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        public ApplicationReferences GetReferences()
        {
            return ObserveApplication(Application, ServiceProvider);
        }
    }

    [DependsOn(typeof(AbpAspNetCoreModule), typeof(AbpAutofacModule))]
    public class TelemetryTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IConfiguration>(new ConfigurationBuilder().Build()));
            context.Services.AddSingleton<TelemetryCompletion>();
            context.Services.Replace(ServiceDescriptor.Scoped<ITelemetryActivityEventBuilder, TestActivityEventBuilder>());
            context.Services.Replace(ServiceDescriptor.Singleton(Substitute.For<ITelemetryActivityStorage>()));
            context.Services.Replace(ServiceDescriptor.Singleton(Substitute.For<ITelemetryActivitySender>()));
        }
    }

    public class TelemetryCompletion
    {
        public TaskCompletionSource<bool> ScopeDisposed { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public int ActivityCount;
        public string ActivityName { get; set; }
    }

    public class TestActivityEventBuilder : ITelemetryActivityEventBuilder, IDisposable
    {
        private readonly TelemetryCompletion _completion;

        public TestActivityEventBuilder(TelemetryCompletion completion)
        {
            _completion = completion;
        }

        public Task<ActivityEvent> BuildAsync(ActivityContext context)
        {
            Interlocked.Increment(ref _completion.ActivityCount);
            _completion.ActivityName = context.Current.Get<string>(ActivityPropertyNames.ActivityName);
            return Task.FromResult<ActivityEvent>(null);
        }

        public void Dispose()
        {
            _completion.ScopeDisposed.TrySetResult(true);
        }
    }
}
