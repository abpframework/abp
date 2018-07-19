using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    public static class VirtualFileSystemApplicationBuilderExtensions
    {
        public static void UseVirtualFiles(this IApplicationBuilder app)
        {
            //var options = app.ApplicationServices.GetRequiredService<IOptions<AspNetCoreContentOptions>>().Value;
            //var hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

            //var fileProvider = new FileProviderSubFolderWrapper(
            //    new CompositeFileProvider(
            //        new PhysicalFileProvider(hostingEnvironment.ContentRootPath),
            //        app.ApplicationServices.GetRequiredService<IVirtualFileProvider>()
            //    ),
            //    "/wwwroot",
            //    options.AllowedExtraWebContentFolders.ToArray(),
            //    options.AllowedExtraWebContentFileExtensions.ToArray()
            //);

            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = app.ApplicationServices.GetRequiredService<IHybridWebRootFileProvider>()
                }
            );
        }
    }
}
