using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class AbpWebAssemblyApplicationCreationOptions
    {
        public WebAssemblyHostBuilder HostBuilder { get; }

        public AbpApplicationCreationOptions ApplicationCreationOptions { get; }

        public AbpWebAssemblyApplicationCreationOptions(
            WebAssemblyHostBuilder hostBuilder,
            AbpApplicationCreationOptions applicationCreationOptions)
        {
            HostBuilder = hostBuilder;
            ApplicationCreationOptions = applicationCreationOptions;
        }
    }
}
