using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpApplicationBuilderExtensions
    {
        public static void InitializeApplication([NotNull] this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
            app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().Initialize(app.ApplicationServices);
        }
    }
}
