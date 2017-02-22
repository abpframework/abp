using Volo.Abp.AspNetCore.EmbeddedFiles;

namespace Microsoft.AspNetCore.Builder
{
    public static class EmbeddedFilesApplicationBuilderExtensions
    {
        public static void UseEmbeddedFiles(this IApplicationBuilder app)
        {
            //TODO: Can improve it or create a custom middleware?
            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = new EmbeddedResourceFileProvider(
                        app.ApplicationServices
                    )
                }
            );
        }
    }
}
