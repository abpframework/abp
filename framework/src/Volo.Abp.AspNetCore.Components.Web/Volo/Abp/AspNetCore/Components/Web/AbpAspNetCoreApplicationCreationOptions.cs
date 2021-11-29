namespace Volo.Abp.AspNetCore.Components.Web
{
    public class AbpAspNetCoreApplicationCreationOptions
    {
        public AbpApplicationCreationOptions ApplicationCreationOptions { get; }

        public AbpAspNetCoreApplicationCreationOptions(
            AbpApplicationCreationOptions applicationCreationOptions)
        {
            ApplicationCreationOptions = applicationCreationOptions;
        }
    }
}
