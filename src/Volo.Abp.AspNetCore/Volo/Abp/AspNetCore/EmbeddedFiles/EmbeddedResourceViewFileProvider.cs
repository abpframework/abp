using System;
using Volo.Abp.EmbeddedFiles;
using Volo.DependencyInjection;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    public class EmbeddedResourceViewFileProvider : EmbeddedResourceFileProvider
    {
        public EmbeddedResourceViewFileProvider(IObjectAccessor<IServiceProvider> serviceProvider) 
            : base(serviceProvider)
        {
        }

        protected override bool IsIgnoredFile(EmbeddedFileInfo resource)
        {
            return resource.FileExtension != "cshtml";
        }
    }
}