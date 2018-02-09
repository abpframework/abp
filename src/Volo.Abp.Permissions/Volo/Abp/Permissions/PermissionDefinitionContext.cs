using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.Permissions
{
    public class PermissionDefinitionContext : IPermissionDefinitionContext
    {
        protected Dictionary<string, PermissionDefinition> Permissions { get; }

        public PermissionDefinitionContext(Dictionary<string, PermissionDefinition> permissions)
        {
            Permissions = permissions;
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            return Permissions.GetOrDefault(name);
        }

        public virtual IReadOnlyList<PermissionDefinition> GetAll()
        {
            return Permissions.Values.ToImmutableList();
        }

        public virtual void Add(params PermissionDefinition[] definitions)
        {
            if (definitions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var definition in definitions)
            {
                Permissions[definition.Name] = definition;
            }
        }
    }
}