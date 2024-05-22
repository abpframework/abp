using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class AbpApplicationConfigurationOptions
{
    public ITypeList<IApplicationConfigurationContributor> Contributors { get; }

    public AbpApplicationConfigurationOptions()
    {
        Contributors = new TypeList<IApplicationConfigurationContributor>();
    }
}
