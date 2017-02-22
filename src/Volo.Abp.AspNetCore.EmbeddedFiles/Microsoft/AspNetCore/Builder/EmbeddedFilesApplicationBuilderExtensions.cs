using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.EmbeddedFiles;

namespace Volo.Abp.AspNetCore.Microsoft.AspNetCore.Builder
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
