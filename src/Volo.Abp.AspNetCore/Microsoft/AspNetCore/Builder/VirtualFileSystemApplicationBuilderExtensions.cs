using Microsoft.Extensions.FileProviders;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    //TODO: Volo.Abp.AspNetCore.EmbeddedFiles package should be referenced by the projects using embedded resources..?

    public static class VirtualFileSystemApplicationBuilderExtensions
    {
        public static void UseVirtualFiles(this IApplicationBuilder app)
        {
            //var options = app.ApplicationServices.GetRequiredService<IOptions<VirtualFileSystemOptions>>().Value;

            IFileProvider fileProvider = new AspNetCoreVirtualFileProvider(
                app.ApplicationServices,
                "/wwwroot"
            );
            
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = fileProvider
                }
            );
        }
    }
}
