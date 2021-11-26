using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class ApplicationAuthConfigurationDto
{
    public Dictionary<string, bool> Policies { get; set; }

    public Dictionary<string, bool> GrantedPolicies { get; set; }

    public ApplicationAuthConfigurationDto()
    {
        Policies = new Dictionary<string, bool>();
        GrantedPolicies = new Dictionary<string, bool>();
    }
}
