using System.Threading.Tasks;
using Volo.Abp.Data;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class TestApplicationConfigurationContributor : IApplicationConfigurationContributor
{
    public Task ContributeAsync(ApplicationConfigurationContributorContext context)
    {
        context.ApplicationConfiguration.SetProperty("TestKey", "TestValue");
        return Task.CompletedTask;
    }
}
