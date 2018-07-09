using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Uow;
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

        public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpAuditingMiddleware>();
        }

        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            return app
                .UseAbpExceptionHandling()
                .UseMiddleware<AbpUnitOfWorkMiddleware>();
        }

        public static IApplicationBuilder UseAbpExceptionHandling(this IApplicationBuilder app)
        {
            //Prevent multiple add
            if (app.Properties.ContainsKey("_AbpExceptionHandlingMiddleware_Added")) //TODO: Constant
            {
                return app;
            }

            app.Properties["_AbpExceptionHandlingMiddleware_Added"] = true;
            return app.UseMiddleware<AbpExceptionHandlingMiddleware>();
        }
    }
}
