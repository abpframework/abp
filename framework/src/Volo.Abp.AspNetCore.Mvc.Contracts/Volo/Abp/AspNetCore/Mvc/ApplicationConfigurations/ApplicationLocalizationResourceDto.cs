using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationLocalizationResourceDto
{
    public Dictionary<string, string> Texts { get; set; }
    
    public List<string> BaseResources { get; set; }
}