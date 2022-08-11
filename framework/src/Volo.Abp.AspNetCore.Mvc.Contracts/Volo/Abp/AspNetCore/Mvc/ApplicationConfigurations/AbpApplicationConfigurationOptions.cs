using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class AbpApplicationConfigurationOptions
{
    public List<IApplicationConfigurationContributor> Contributors { get; }

    public AbpApplicationConfigurationOptions()
    {
        Contributors = new List<IApplicationConfigurationContributor>();
    }
}
