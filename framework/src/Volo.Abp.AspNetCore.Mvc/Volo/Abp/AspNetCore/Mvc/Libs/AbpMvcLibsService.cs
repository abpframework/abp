using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var app = context.GetApplicationBuilder();
            app.Use(async (httpContext, next) =>
            {
                if (!await HandleCheckLibsAsyncOnceAsync(httpContext))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "text/html";
                    await httpContext.Response.WriteAsync(
                        "<html>" +
                        "   <head>" +
                        "       <title>ABP MVC Libs Error</title>" +
                        "   </head>" +
                        "   <body>" +
                        "       <h1>ABP MVC Libs folder(wwwroot/libs) does not exist or empty!</h1>" +
                        "       <p>Please make sure you have run the <b>abp install-libs</b> command.</p>" +
                        "       <p>For more information, see <a href='https://abp.io/docs/latest/CLI#install-libs'>CLI install-libs</a> document.</p>" +
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

    protected virtual Task<bool> HandleCheckLibsAsyncOnceAsync(HttpContext httpContext)
    {
        if (_checkLibsTask == null)
        {
            _checkLibsTask = HandleCheckLibsAsync(httpContext);
        }

        return _checkLibsTask;
    }

    protected virtual Task<bool> HandleCheckLibsAsync(HttpContext httpContext)
    {
        var fileProvider = new PhysicalFileProvider(httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().WebRootPath);
        var libsFolder = fileProvider.GetDirectoryContents("/libs");
        if (!libsFolder.Exists || !libsFolder.Any())
        {
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<AbpMvcLibsService>>();
            logger.LogError("The 'wwwroot/libs' folder does not exist or empty!");
            return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }
}
