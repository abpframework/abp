using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ISingletonDependency
    {
        public abstract void Define(IPermissionDefinitionContext context);
    }
}