using Volo.Abp.AspNetCore.EmbeddedFiles;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    public static class VirtualFileSystemApplicationBuilderExtensions
    {
        public static void UseVirtualFiles(this IApplicationBuilder app)
        {
            //TODO: Can improve it or create a custom middleware?
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new AspNetCoreVirtualFileProvider(
                        app.ApplicationServices
                    )
                }
            );
        }
    }
}
