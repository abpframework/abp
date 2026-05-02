using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.BackgroundWorkers;

public class InMemoryDynamicBackgroundWorker_Registration_Tests
{
    [Fact]
    public async Task Should_Not_Be_Auto_Registered_To_DI_Container()
    {
        // Regression guard: IBackgroundWorker derives from ISingletonDependency, so without
        // [DisableConventionalRegistration] the conventional registration would register
        // InMemoryDynamicBackgroundWorker as a Singleton. Its constructor takes a `string
        // workerName` parameter that the DI container cannot resolve, which broke any host
        // running ServiceCollection validation (e.g. ASP.NET Core in Development, where
        // WebApplicationBuilder.Build() enables ValidateOnBuild).
        using var application = await AbpApplicationFactory.CreateAsync<AbpBackgroundWorkersModule>();

        application.Services
            .Any(d => d.ServiceType == typeof(InMemoryDynamicBackgroundWorker))
            .ShouldBeFalse(
                "InMemoryDynamicBackgroundWorker is created on demand by DefaultDynamicBackgroundWorkerManager " +
                "and must not be auto-registered as a service.");

        // Building a fresh provider with ValidateOnBuild = true must not throw.
        await using var validatingProvider = application.Services.BuildServiceProvider(
            new ServiceProviderOptions { ValidateOnBuild = true });
        validatingProvider.ShouldNotBeNull();
    }
}
