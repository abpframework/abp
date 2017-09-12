namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAspNetCoreMvcOptions
    {
        public AppServiceControllerOptions AppServiceControllers { get; }

        public AbpAspNetCoreMvcOptions()
        {
            AppServiceControllers = new AppServiceControllerOptions();
        }
    }
}