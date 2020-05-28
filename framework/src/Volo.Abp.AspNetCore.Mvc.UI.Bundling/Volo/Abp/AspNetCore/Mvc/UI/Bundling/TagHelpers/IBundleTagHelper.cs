using Microsoft.AspNetCore.Mvc.Rendering;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    public interface IBundleTagHelper
    {
        string GetNameOrNull();

        ViewContext ViewContext { get; set; }
    }
}
