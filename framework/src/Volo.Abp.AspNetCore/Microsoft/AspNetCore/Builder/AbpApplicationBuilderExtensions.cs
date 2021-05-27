using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Security;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.AspNetCore.Tracing;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class AbpApplicationBuilderExtensions
    {
        private const string ExceptionHandlingMiddlewareMarker = "_AbpExceptionHandlingMiddleware_Added";

        public static void InitializeApplication([NotNull] this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
            var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                application.Shutdown();
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            application.Initialize(app.ApplicationServices);
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

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpCorrelationIdMiddleware>();
        }

        public static IApplicationBuilder UseAbpRequestLocalization(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AbpRequestLocalizationMiddleware>();
        }

        public static IApplicationBuilder UseAbpExceptionHandling(this IApplicationBuilder app)
        {
            if (app.Properties.ContainsKey(ExceptionHandlingMiddlewareMarker))
            {
                return app;
            }

            app.Properties[ExceptionHandlingMiddlewareMarker] = true;
            return app.UseMiddleware<AbpExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseAbpClaimsMap(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AbpClaimsMapMiddleware>();
        }

        public static IApplicationBuilder UseAbpSecurityHeaders(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AbpSecurityHeadersMiddleware>();
        }
    }
}
