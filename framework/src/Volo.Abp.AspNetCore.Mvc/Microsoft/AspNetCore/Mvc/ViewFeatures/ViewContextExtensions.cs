using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures;

public static class ViewContextExtensions
{
    public static IUrlHelper GetUrlHelper(this ViewContext viewContext)
    {
        var urlHelperFactory = viewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        return urlHelperFactory.GetUrlHelper(viewContext);
    }
}
