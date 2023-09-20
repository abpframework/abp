using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationLocalizationRequestDto
{
    [Required]
    public string CultureName { get; set; } = default!;
    
    public bool OnlyDynamics { get; set; }
}