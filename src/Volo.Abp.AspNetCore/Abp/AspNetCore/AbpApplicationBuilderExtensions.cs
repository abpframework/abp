using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp.AspNetCore
{
    public static class AbpApplicationBuilderExtensions
    {
        public static void InitializeAbpApplication(this IApplicationBuilder app) //TODO: Simply rename to InitializeApplication?
        {
            var abpApplication = app.ApplicationServices.GetRequiredService<IAbpApplication>();

            app.ApplicationServices.GetRequiredService<ApplicationBuilderAccessor>().App = app;

            abpApplication.Initialize(app.ApplicationServices);
        }
    }

    public class ApplicationBuilderAccessor : ISingletonDependency
    {
        public IApplicationBuilder App { get; set; }
    }
}
