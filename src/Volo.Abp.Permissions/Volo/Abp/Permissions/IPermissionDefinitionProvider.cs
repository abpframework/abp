using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Permissions
{
    public interface IPermissionDefinitionProvider : ISingletonDependency
    {
        void Define(IPermissionDefinitionContext context);
    }
}