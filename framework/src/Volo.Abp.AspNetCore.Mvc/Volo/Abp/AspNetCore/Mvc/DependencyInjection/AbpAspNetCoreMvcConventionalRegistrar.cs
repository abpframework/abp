using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.DependencyInjection;

public class AbpAspNetCoreMvcConventionalRegistrar : DefaultConventionalRegistrar
{
    protected override bool IsConventionalRegistrationDisabled(Type type)
    {
        return !IsMvcService(type) || base.IsConventionalRegistrationDisabled(type);
    }

    protected virtual bool IsMvcService(Type type)
    {
        return IsController(type) ||
               IsPageModel(type) ||
               IsViewComponent(type);
    }

    private static bool IsPageModel(Type type)
    {
        return typeof(PageModel).IsAssignableFrom(type) || type.IsDefined(typeof(PageModelAttribute), true);
    }

    private static bool IsController(Type type)
    {
        return typeof(Controller).IsAssignableFrom(type) || type.IsDefined(typeof(ControllerAttribute), true);
    }

    private static bool IsViewComponent(Type type)
    {
        return typeof(ViewComponent).IsAssignableFrom(type) || type.IsDefined(typeof(ViewComponentAttribute), true);
    }

    protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
    {
        return ServiceLifetime.Transient;
    }
}
