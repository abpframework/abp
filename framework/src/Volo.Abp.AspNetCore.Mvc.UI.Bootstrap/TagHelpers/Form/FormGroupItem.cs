namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class FormGroupItem
{
    public string HtmlContent { get; set; } = default!;

    public int Order { get; set; }

    public string PropertyName { get; set; } = default!;
}
