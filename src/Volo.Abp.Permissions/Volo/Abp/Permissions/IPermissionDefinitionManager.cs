using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public interface IPermissionDefinitionManager
    {
        [NotNull]
        PermissionDefinition Get([NotNull] string name);

        IReadOnlyList<PermissionDefinition> GetAll();
    }
}