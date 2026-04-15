using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.TestApp.Application.Dto;

/// <summary>
/// A documented DTO for testing type and property descriptions.
/// </summary>
[Description("Documented DTO description from attribute")]
[Display(Name = "Documented DTO")]
public class DocumentedDto
{
    /// <summary>
    /// The name of the documented item.
    /// </summary>
    [Description("Name description from attribute")]
    [Display(Name = "Item Name")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// The value of the documented item.
    /// </summary>
    [Description("Value description from attribute")]
    public int Value { get; set; }
}
