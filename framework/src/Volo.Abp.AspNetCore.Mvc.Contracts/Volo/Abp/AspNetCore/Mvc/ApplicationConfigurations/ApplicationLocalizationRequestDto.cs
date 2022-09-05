using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class ApplicationLocalizationRequestDto
{
    [Required]
    public string Culture { get; set; }
}