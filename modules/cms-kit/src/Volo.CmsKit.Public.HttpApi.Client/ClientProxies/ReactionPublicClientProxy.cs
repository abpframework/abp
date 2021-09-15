// This file is part of ReactionPublicClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.CmsKit.Public.Reactions;

// ReSharper disable once CheckNamespace
namespace Volo.CmsKit.Public.Reactions.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IReactionPublicAppService), typeof(ReactionPublicClientProxy))]
    public partial class ReactionPublicClientProxy : ClientProxyBase<IReactionPublicAppService>, IReactionPublicAppService
    {
    }
}
