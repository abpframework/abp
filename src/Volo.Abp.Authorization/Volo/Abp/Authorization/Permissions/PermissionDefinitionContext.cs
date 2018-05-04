using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionDefinitionContext : IPermissionDefinitionContext
    {
        internal Dictionary<string, PermissionGroupDefinition> Groups { get; }

        internal PermissionDefinitionContext()
        {
            Groups = new Dictionary<string, PermissionGroupDefinition>();
        }

        public virtual PermissionGroupDefinition AddGroup(string name, ILocalizableString displayName = null)
        {
            Check.NotNull(name, nameof(name));

            if (Groups.ContainsKey(name))
            {
                throw new AbpException($"There is already an existing permission group with name: {name}");
            }

            return Groups[name] = new PermissionGroupDefinition(name, displayName);
        }
    }
}