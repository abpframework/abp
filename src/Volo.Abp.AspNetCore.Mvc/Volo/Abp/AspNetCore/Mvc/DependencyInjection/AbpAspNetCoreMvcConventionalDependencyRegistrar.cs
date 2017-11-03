using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.DependencyInjection
{
    public class AbpAspNetCoreMvcConventionalRegistrar : DefaultConventionalRegistrar
    {
        protected override ServiceLifetime? GetServiceLifetimeFromClassHierarcy(Type type)
        {
            var lifeTime = base.GetServiceLifetimeFromClassHierarcy(type);
            if (lifeTime != null)
            {
                return lifeTime;
            }

            if (IsController(type) ||
                IsPageModel(type) ||
                IsViewComponent(type))
            {
                return ServiceLifetime.Transient;
            }

            return null;
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
    }
}
