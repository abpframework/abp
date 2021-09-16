// This file is part of MediaDescriptorClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.MediaDescriptors;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.MediaDescriptors.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IMediaDescriptorAppService), typeof(MediaDescriptorClientProxy))]
    public partial class MediaDescriptorClientProxy : ClientProxyBase<IMediaDescriptorAppService>, IMediaDescriptorAppService
    {
    }
}
