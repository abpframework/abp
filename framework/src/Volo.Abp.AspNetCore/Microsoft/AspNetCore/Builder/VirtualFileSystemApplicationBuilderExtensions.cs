using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    public static class VirtualFileSystemApplicationBuilderExtensions
    {
        public static void UseVirtualFiles(this IApplicationBuilder app)
        {
            IFileProvider fileProvider = new FileProviderSubFolderWrapper(
                app.ApplicationServices.GetRequiredService<IVirtualFileProvider>(),
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
