using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public interface IApplicationConfigurationContributor
{
    Task ContributeAsync(ApplicationConfigurationContributorContext context);
}
