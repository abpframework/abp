using System;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.VirtualFileSystem
{
    public class AspNetCoreVirtualViewFileProvider : AspNetCoreVirtualFileProvider
    {
        public AspNetCoreVirtualViewFileProvider(IObjectAccessor<IServiceProvider> serviceProviderAccessor) 
            : base(serviceProviderAccessor)
        {
        }
    }
}