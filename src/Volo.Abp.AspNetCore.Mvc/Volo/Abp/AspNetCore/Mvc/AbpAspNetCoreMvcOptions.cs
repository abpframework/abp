namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAspNetCoreMvcOptions
    {
        //TODO: Rename to ConventionalControllers
        public AppServiceControllerOptions AppServiceControllers { get; }

        public AbpAspNetCoreMvcOptions()
        {
            AppServiceControllers = new AppServiceControllerOptions();
        }
    }
}