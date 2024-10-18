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
                if (!await CheckLibsAsyncOnceAsync(httpContext))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "text/html";
                    await httpContext.Response.WriteAsync(
                        "<html>" +
                        "   <head>" +
                        "       <title>Error - The Libs folder is missing!</title>" +
                        "   </head>" +
                        "   <body>" +
                        "       <h1> &#9888;&#65039; The Libs folder under the <code style='background-color: #e7e7e7;'>wwwroot/libs</code> directory is empty!</h1>" +
                        "       <p>The Libs folder contains mandatory NPM Packages for running the project.</p>" +
                        "       <p>Make sure you run the <code style='background-color: #e7e7e7;'>abp install-libs</code> CLI tool command.</p>" +
                        "       <p>For more information, check out the <a href='https://abp.io/docs/latest/CLI#install-libs'>ABP CLI documentation</a></p>" +
                        "   </body>" +
                        "</html>",
                        Encoding.UTF8
                    );
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
                logger.LogWarning("The 'WebRootPath' is not set! The 'CheckLibs' feature is disabled!");
                return Task.FromResult(true);
            }

            var fileProvider = new PhysicalFileProvider(webHostEnvironment.WebRootPath);
            var libsFolder = fileProvider.GetDirectoryContents("/libs");
            if (!libsFolder.Exists || !libsFolder.Any())
            {
                logger.LogError("The 'wwwroot/libs' folder does not exist or empty!");
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
