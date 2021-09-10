// This file is part of MediaDescriptorAdminClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Admin.MediaDescriptors;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Admin.MediaDescriptors.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IMediaDescriptorAdminAppService), typeof(MediaDescriptorAdminClientProxy))]
    public partial class MediaDescriptorAdminClientProxy : ClientProxyBase<IMediaDescriptorAdminAppService>, IMediaDescriptorAdminAppService
    {
    }
}
