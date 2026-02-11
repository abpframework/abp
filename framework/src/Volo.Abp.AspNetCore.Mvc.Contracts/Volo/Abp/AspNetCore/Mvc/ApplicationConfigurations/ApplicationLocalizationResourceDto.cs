using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class ApplicationLocalizationResourceDto
{
    public Dictionary<string, string> Texts { get; set; }

    public string[] BaseResources { get; set; } = default!;

    public ApplicationLocalizationResourceDto()
    {
        Texts = new Dictionary<string, string>();
    }
}