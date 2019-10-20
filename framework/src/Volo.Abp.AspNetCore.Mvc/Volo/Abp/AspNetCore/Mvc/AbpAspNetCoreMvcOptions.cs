using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAspNetCoreMvcOptions
    {
        public AbpConventionalControllerOptions ConventionalControllers { get; }

        public AbpAspNetCoreMvcOptions()
        {
            ConventionalControllers = new AbpConventionalControllerOptions();
        }
    }
}