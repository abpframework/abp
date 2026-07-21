using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundWorkers;

public class InMemoryDynamicBackgroundWorker_Registration_Tests
{
    // Reproduces the original failure: ASP.NET Core in Development enables ValidateOnBuild,
    // and InMemoryDynamicBackgroundWorker has a `string workerName` constructor parameter
    // that DI cannot resolve. Without [DisableConventionalRegistration] this throws:
    //   Unable to resolve service for type 'System.String' while attempting to activate
    //   'Volo.Abp.BackgroundWorkers.InMemoryDynamicBackgroundWorker'.
    [Fact]
    public async Task BuildServiceProvider_With_ValidateOnBuild_Should_Not_Throw()
    {
        using var application = await AbpApplicationFactory.CreateAsync<AbpBackgroundWorkersModule>();

        var act = () => application.Services.BuildServiceProvider(
            new ServiceProviderOptions { ValidateOnBuild = true });

        act.ShouldNotThrow();
    }

    // Verifies the fix: InMemoryDynamicBackgroundWorker is created on demand by
    // DefaultDynamicBackgroundWorkerManager (`new InMemoryDynamicBackgroundWorker(...)`)
    // and must stay out of the conventional registration loop.
    [Fact]
    public async Task InMemoryDynamicBackgroundWorker_Should_Not_Be_Registered_As_Service()
    {
        using var application = await AbpApplicationFactory.CreateAsync<AbpBackgroundWorkersModule>();

        application.Services.ShouldNotContain(d => d.ServiceType == typeof(InMemoryDynamicBackgroundWorker));
    }
}
