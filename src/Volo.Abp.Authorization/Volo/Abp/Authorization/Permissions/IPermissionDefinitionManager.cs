using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionDefinitionManager
    {
        [NotNull]
        PermissionDefinition Get([NotNull] string name);

        IReadOnlyList<PermissionDefinition> GetAll();
    }
}