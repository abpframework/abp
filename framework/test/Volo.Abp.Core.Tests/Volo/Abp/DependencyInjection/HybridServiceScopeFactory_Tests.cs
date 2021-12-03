using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.DependencyInjection;

public class HybridServiceScopeFactory_Tests
{
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

    private class MyService : ITransientDependency, IDisposable
    {
        public static int DisposeCount { get; private set; }

        public void Dispose()
        {
            ++DisposeCount;
        }
    }
}
