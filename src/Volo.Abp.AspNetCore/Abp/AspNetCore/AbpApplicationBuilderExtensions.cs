using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpApplicationBuilderExtensions
    {
        public static void InitializeAbpApplication(this IApplicationBuilder app) //TODO: Simply rename to InitializeApplication?
        {
            var abpApplication = app.ApplicationServices.GetRequiredService<AbpApplication>();

            app.ApplicationServices.GetRequiredService<ApplicationBuilderAccessor>().App = app;

            abpApplication.Initialize(app.ApplicationServices);
        }
    }

    public class ApplicationBuilderAccessor : ISingletonDependency
    {
        public IApplicationBuilder App { get; set; }
    }
}
