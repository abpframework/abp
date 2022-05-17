using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.DependencyInjection;

public class HybridServiceScopeFactory_Tests
{
    [Fact]
    public async Task Should_Use_Default_ServiceScopeFactory_By_Default_Async()
    {
        using (var application = await AbpApplicationFactory.CreateAsync<IndependentEmptyModule>())
        {
            application.Services.AddType(typeof(MyServiceAsync));

            await application.InitializeAsync();

            var serviceScopeFactory = application.ServiceProvider.GetRequiredService<IHybridServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<MyServiceAsync>();
            }

            MyServiceAsync.DisposeCount.ShouldBe(1);
        }
    }

    [Fact]
    public void Should_Use_Default_ServiceScopeFactory_By_Default()
    {
        using (var application = AbpApplicationFactory.Create<IndependentEmptyModule>())
        {
            application.Services.AddType(typeof(MyService));

            application.Initialize();

            var serviceScopeFactory = application.ServiceProvider.GetRequiredService<IHybridServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<MyService>();
            }

            MyService.DisposeCount.ShouldBe(1);
        }
    }

    private class MyServiceAsync : ITransientDependency, IDisposable
    {
        public static int DisposeCount { get; private set; }

        public void Dispose()
        {
            ++DisposeCount;
        }
    }

    private class MyService : ITransientDependency, IDisposable
    {
        public static int DisposeCount { get; private set; }

        public void Dispose()
        {
            ++DisposeCount;
        }
    }
}
