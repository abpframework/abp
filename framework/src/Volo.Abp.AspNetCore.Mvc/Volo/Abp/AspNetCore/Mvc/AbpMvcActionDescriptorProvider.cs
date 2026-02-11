using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Volo.Abp.AspNetCore.Filters;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationModels;

public class AbpMvcActionDescriptorProvider : IActionDescriptorProvider
{
    public virtual int Order => -1000 + 10;

    public virtual void OnProvidersExecuting(ActionDescriptorProviderContext context)
    {
    }

    public virtual void OnProvidersExecuted(ActionDescriptorProviderContext context)
    {
        foreach (var action in context.Results.Where(x => x is ControllerActionDescriptor).Cast<ControllerActionDescriptor>())
        {
            var disableAbpFeaturesAttribute = action.ControllerTypeInfo.GetCustomAttribute<DisableAbpFeaturesAttribute>(true);
            if (disableAbpFeaturesAttribute != null && disableAbpFeaturesAttribute.DisableMvcFilters)
            {
                action.FilterDescriptors.RemoveAll(x => x.Filter is ServiceFilterAttribute serviceFilterAttribute &&
                                                        typeof(IAbpFilter).IsAssignableFrom(serviceFilterAttribute.ServiceType));
            }
        }
    }
}
