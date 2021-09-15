// This file is part of DocsProjectClientProxy, you can customize it here
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Docs.Projects;

// ReSharper disable once CheckNamespace
namespace Volo.Docs.Projects.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IProjectAppService), typeof(DocsProjectClientProxy))]
    public partial class DocsProjectClientProxy : ClientProxyBase<IProjectAppService>, IProjectAppService
    {
    }
}
