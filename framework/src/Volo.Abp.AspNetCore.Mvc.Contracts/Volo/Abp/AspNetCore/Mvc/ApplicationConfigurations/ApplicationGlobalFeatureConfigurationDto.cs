using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class ApplicationGlobalFeatureConfigurationDto
{
    public HashSet<string> EnabledFeatures { get; set; }

    public Dictionary<string, List<string>> ModuleEnabledFeatures { get; set; }

    public ApplicationGlobalFeatureConfigurationDto()
    {
        EnabledFeatures = new HashSet<string>();
        ModuleEnabledFeatures = new Dictionary<string, List<string>>();
    }
}
