using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.DropdownsDemo;

public class DropDownDemoDemoModel
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}
