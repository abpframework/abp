using System;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.VirtualFileSystem
{
    //TODO: Move to Volo.Abp.AspNetCore.Mvc project!
    public class AspNetCoreVirtualViewFileProvider : AspNetCoreVirtualFileProvider
    {
        public AspNetCoreVirtualViewFileProvider(IObjectAccessor<IServiceProvider> serviceProviderAccessor) 
            : base(serviceProviderAccessor)
        {
        }
    }
}