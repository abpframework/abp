namespace Volo.Abp.AspNetCore.Components.UI
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
