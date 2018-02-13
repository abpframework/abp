using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionDefinitionContext
    {
        PermissionDefinition GetOrNull([NotNull] string name);

        PermissionDefinition Add([NotNull] string name);
    }
}