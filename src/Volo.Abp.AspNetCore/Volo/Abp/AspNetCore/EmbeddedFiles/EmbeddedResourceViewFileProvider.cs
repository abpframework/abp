using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EmbeddedFiles;

namespace Volo.Abp.AspNetCore.EmbeddedFiles
{
    //TODO: Remove to Volo.Abp.AspNetCore.Mvc project!
    public class EmbeddedResourceViewFileProvider : EmbeddedResourceFileProvider
    {
        public EmbeddedResourceViewFileProvider(IObjectAccessor<IServiceProvider> serviceProviderAccessor) 
            : base(serviceProviderAccessor)
        {
        }

        protected override bool IsIgnoredFile(EmbeddedFileInfo resource)
        {
            return resource.FileExtension != "cshtml";
        }
    }
}