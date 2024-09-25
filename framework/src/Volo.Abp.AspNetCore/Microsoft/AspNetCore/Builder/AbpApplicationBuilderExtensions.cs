using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticAssets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Security;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.AspNetCore.Tracing;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder;

public static class AbpApplicationBuilderExtensions
{
    private const string ExceptionHandlingMiddlewareMarker = "_AbpExceptionHandlingMiddleware_Added";

    public async static Task InitializeApplicationAsync([NotNull] this IApplicationBuilder app)
    {
        Check.NotNull(app, nameof(app));

        app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
        var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
        var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStopping.Register(() =>
        {
            AsyncHelper.RunSync(() => application.ShutdownAsync());
        });

        applicationLifetime.ApplicationStopped.Register(() =>
        {
            application.Dispose();
        });

        await application.InitializeAsync(app.ApplicationServices);
    }

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

    public static IApplicationBuilder UseAbpRequestLocalization(this IApplicationBuilder app,
        Action<RequestLocalizationOptions>? optionsAction = null)
    {
        app.ApplicationServices
            .GetRequiredService<IAbpRequestLocalizationOptionsProvider>()
            .InitLocalizationOptions(optionsAction);

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

    [Obsolete("Replace with AbpClaimsTransformation")]
    public static IApplicationBuilder UseAbpClaimsMap(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AbpClaimsMapMiddleware>();
    }

    public static IApplicationBuilder UseAbpSecurityHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AbpSecurityHeadersMiddleware>();
    }

    public static IApplicationBuilder UseDynamicClaims(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AbpDynamicClaimsMiddleware>();
    }

    /// <summary>
    /// MapAbpStaticAssets is used to serve the files from the abp virtual file system embedded resources(js/css) and call the MapStaticAssets.
    /// </summary>
    public static StaticAssetsEndpointConventionBuilder MapAbpStaticAssets(this WebApplication app, string? staticAssetsManifestPath = null)
    {
        return app.As<IApplicationBuilder>().MapAbpStaticAssets(staticAssetsManifestPath);
    }

    /// <summary>
    /// MapAbpStaticAssets is used to serve the files from the abp virtual file system embedded resources(js/css) and call the MapStaticAssets.
    /// </summary>
    public static StaticAssetsEndpointConventionBuilder MapAbpStaticAssets(this IApplicationBuilder app, string? staticAssetsManifestPath = null)
    {
        if (app is not IEndpointRouteBuilder endpoints)
        {
            throw new AbpException("The app(IApplicationBuilder) is not an IEndpointRouteBuilder.");
        }

        app.UseVirtualStaticFiles();

        var options = app.ApplicationServices.GetRequiredService<IOptions<AbpAspNetCoreContentOptions>>().Value;
        foreach (var folder in options.AllowedExtraWebContentFolders)
        {
            app.UseVirtualStaticFiles(folder);
        }

        return endpoints.MapStaticAssets(staticAssetsManifestPath);
    }

    /// <summary>
    /// This static file provider is used to serve the files from the abp virtual file system embedded resources(js/css).
    /// It will not serve the files from the application's wwwroot folder.
    /// </summary>
    public static IApplicationBuilder UseVirtualStaticFiles(this IApplicationBuilder app)
    {
        app.UseStaticFiles(new StaticFileOptions()
        {
            ContentTypeProvider = app.ApplicationServices.GetRequiredService<AbpFileExtensionContentTypeProvider>(),
            FileProvider = new WebContentFileProvider(
                app.ApplicationServices.GetRequiredService<IVirtualFileProvider>(),
                new EmptyHostingEnvironment(),
                app.ApplicationServices.GetRequiredService<IOptions<AbpAspNetCoreContentOptions>>()
            )
        });

        return app;
    }

    /// <summary>
    /// This static file provider is used to serve the files from the <param name="folder">folder</param>.
    /// </summary>
    public static IApplicationBuilder UseVirtualStaticFiles(this IApplicationBuilder app, string folder)
    {
        folder = folder.TrimStart('/').TrimEnd('/');

        var root = Path.Combine(app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().ContentRootPath, folder);
        if (!Directory.Exists(root))
        {
            return app;
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = app.ApplicationServices.GetRequiredService<AbpFileExtensionContentTypeProvider>(),
            FileProvider = new PhysicalFileProvider(root),
            RequestPath = $"/{folder}"
        });

        return app;
    }
}
