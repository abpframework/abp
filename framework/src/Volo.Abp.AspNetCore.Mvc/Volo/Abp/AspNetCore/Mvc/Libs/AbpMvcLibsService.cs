using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.RazorViews;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Libs;

public class AbpMvcLibsService : IAbpMvcLibsService, ITransientDependency
{
    private Task<bool>? _checkLibsTask;

    public virtual void CheckLibs(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpMvcLibsOptions>>().Value;
        if (options.CheckLibs)
        {
            var app = context.GetApplicationBuilderOrNull();
            if (app == null)
            {
                var logger = context.ServiceProvider.GetRequiredService<ILogger<AbpMvcLibsService>>();
                logger.LogWarning($"The {nameof(IApplicationBuilder)} is not available. The 'CheckLibs' feature is disabled!");
                return;
            }

            app.Use(async (httpContext, next) =>
            {
                if (!httpContext.Request.IsAjax() && !await CheckLibsAsyncOnceAsync(httpContext))
                {
                    var errorPage = new AbpMvcLibsErrorPage();
                    await errorPage.ExecuteAsync(httpContext);
                    return;
                }

                await next(httpContext);
            });
        }
    }

    protected virtual Task<bool> CheckLibsAsyncOnceAsync(HttpContext httpContext)
    {
        if (_checkLibsTask == null)
        {
            _checkLibsTask = CheckLibsAsync(httpContext);
        }

        return _checkLibsTask;
    }

    protected virtual Task<bool> CheckLibsAsync(HttpContext httpContext)
    {
        var logger = httpContext.RequestServices.GetRequiredService<ILogger<AbpMvcLibsService>>();
        try
        {
            var webHostEnvironment = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            if (webHostEnvironment.WebRootPath.IsNullOrWhiteSpace())
            {
                logger.LogInformation("The 'WebRootPath' is not set, The 'CheckLibs' feature not needed.");
                return Task.FromResult(true);
            }

            var fileProvider = new PhysicalFileProvider(webHostEnvironment.WebRootPath);
            var libsFolder = fileProvider.GetDirectoryContents("/libs");
            if (!libsFolder.Exists || !libsFolder.Any())
            {
                logger.LogError("The 'wwwroot/libs' folder does not exist or empty! " +
                                "If your application does not use any client-side libraries, you can disable this check by setting 'AbpMvcLibsOptions.CheckLibs' to 'false'");
                return Task.FromResult(false);
            }
        }
        catch (Exception e)
        {
            // In case of any exception, log it and return true to prevent crashing the application.
            logger.LogError(e, "An error occurred while checking the libs folder!");
        }

        return Task.FromResult(true);
    }
}
