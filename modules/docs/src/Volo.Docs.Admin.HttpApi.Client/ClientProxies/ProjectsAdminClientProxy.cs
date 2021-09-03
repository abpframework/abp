using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Docs.Admin.Projects;

// ReSharper disable once CheckNamespace
namespace Volo.Docs.Admin.ClientProxies
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IProjectAdminAppService), typeof(ProjectsAdminClientProxy))]
    public partial class ProjectsAdminClientProxy : ClientProxyBase<IProjectAdminAppService>, IProjectAdminAppService
    {
    }
}
