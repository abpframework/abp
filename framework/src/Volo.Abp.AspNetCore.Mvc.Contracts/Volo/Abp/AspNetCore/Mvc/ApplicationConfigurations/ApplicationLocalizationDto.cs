using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationLocalizationDto
{
    public Dictionary<string, ApplicationLocalizationResourceDto> Resources { get; set; }
}