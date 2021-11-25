using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class ApplicationFeatureConfigurationDto
{
    public Dictionary<string, string> Values { get; set; }

    public ApplicationFeatureConfigurationDto()
    {
        Values = new Dictionary<string, string>();
    }
}
