using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Volo.Abp.Permissions
{
    public class PermissionDefinitionContext : IPermissionDefinitionContext
    {
        internal Dictionary<string, PermissionDefinition> Permissions { get; }

        public PermissionDefinitionContext()
        {
            Permissions = new Dictionary<string, PermissionDefinition>();
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

            return Permissions.GetOrDefault(name);
        }

        public virtual IReadOnlyList<PermissionDefinition> GetAll()
        {
            return Permissions.Values.ToImmutableList();
        }

        public virtual PermissionDefinition Add(string name)
        {
            Check.NotNull(name, nameof(name));

            if (Permissions.ContainsKey(name))
            {
                throw new AbpException($"There is already an existing permission with name: {name}");
            }

            return Permissions[name] = new PermissionDefinition(name);
        }

        internal void HandleNewChildren()
        {
            Permissions.Values
                .SelectMany(p => p.Children)
                .ToList()
                .ForEach(AddPermissionRecursively);
        }

        private void AddPermissionRecursively(PermissionDefinition permission)
        {
            //Prevent multiple adding of same named permission.
            if (Permissions.TryGetValue(permission.Name, out var existingPermission))
            {
                if (existingPermission != permission)
                {
                    throw new AbpException("Duplicate permission name detected for " + permission.Name);
                }
            }
            else
            {
                Permissions[permission.Name] = permission;
            }

            //Add child permissions (recursive call)
            foreach (var childPermission in permission.Children)
            {
                AddPermissionRecursively(childPermission);
            }
        }
    }
}