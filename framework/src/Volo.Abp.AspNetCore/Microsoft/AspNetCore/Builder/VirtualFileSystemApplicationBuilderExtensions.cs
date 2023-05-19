using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

public static class VirtualFileSystemApplicationBuilderExtensions
{
    [Obsolete("Use UseStaticFiles() instead. UseVirtualFiles is not needed anymore.")]
    public static IApplicationBuilder UseVirtualFiles(this IApplicationBuilder app, Action<StaticFileOptions> configure = null)
    {
        if (configure != null)
        {
            configure(app.ApplicationServices.GetRequiredService<IOptions<StaticFileOptions>>().Value);
        }

        return app.UseStaticFiles();
    }
}
