using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class TestApplicationConfigurationContributor : IApplicationConfigurationContributor, ITransientDependency
{
    public Task ContributeAsync(ApplicationConfigurationContributorContext context)
    {
        context.ApplicationConfiguration.SetProperty("TestKey", "TestValue");
        return Task.CompletedTask;
    }
}
