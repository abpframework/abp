using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionDefinitionProvider : ISingletonDependency
    {
        void Define(IPermissionDefinitionContext context);
    }
}