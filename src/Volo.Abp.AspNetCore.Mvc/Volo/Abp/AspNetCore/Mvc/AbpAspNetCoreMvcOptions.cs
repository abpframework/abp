using Volo.Abp.AspNetCore.Mvc.Conventions;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAspNetCoreMvcOptions
    {
        public ConventionalControllerOptions ConventionalControllers { get; }

        public AbpAspNetCoreMvcOptions()
        {
            ConventionalControllers = new ConventionalControllerOptions();
        }
    }
}