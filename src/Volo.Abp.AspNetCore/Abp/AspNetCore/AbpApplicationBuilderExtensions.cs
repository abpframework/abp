using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpApplicationBuilderExtensions
    {
        public static void InitializeApplication(this IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Object = app;
            app.ApplicationServices.GetRequiredService<AbpApplication>().Initialize(app.ApplicationServices);
        }
    }
}
