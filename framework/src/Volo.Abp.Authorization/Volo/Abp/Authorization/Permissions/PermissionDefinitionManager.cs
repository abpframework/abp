using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionDefinitionManager : IPermissionDefinitionManager, ISingletonDependency
    {
        protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;

        protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

        protected AbpPermissionOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;

        public PermissionDefinitionManager(
            IOptions<AbpPermissionOptions> options,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Options = options.Value;

            _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(
                CreatePermissionDefinitions,
                isThreadSafe: true
            );

            _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(
                CreatePermissionGroupDefinitions,
                isThreadSafe: true
            );
        }

        public PermissionGroupDefinition GetGroup( string name )
        {
            if( !PermissionGroupDefinitions.ContainsKey( name ) )
            {
                throw new AbpException($"Could not find a permission definition group with the given name: {name}");
            }

            return PermissionGroupDefinitions[name];
        }

        public virtual PermissionDefinition Get(string name)
        {
            var permission = GetOrNull(name);

            if (permission == null)
            {
                throw new AbpException("Undefined permission: " + name);
            }

            return permission;
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

            return PermissionDefinitions.GetOrDefault(name);
        }

        public virtual IReadOnlyList<PermissionDefinition> GetPermissions()
        {
            return PermissionDefinitions.Values.ToImmutableList();
        }

        public IReadOnlyList<PermissionGroupDefinition> GetGroups()
        {
            return PermissionGroupDefinitions.Values.ToImmutableList();
        }
        
        public PermissionGroupDefinition GetGroup( string name )
        {
            if( !PermissionGroupDefinitions.ContainsKey( name ) )
            {
                throw new AbpException($"Could not find a permission definition group with the given name: {name}");
            }

            return PermissionGroupDefinitions[name];
        }
        
        public PermissionDefinition AddGroupPermission( PermissionGroupDefinition group, string permissionName, ILocalizableString displayName )
        {
	        // create new group
	        PermissionDefinition groupPermission = group.AddPermission( permissionName, displayName );;

	        // add to permission list
	        PermissionDefinitions.Add( permissionName, groupPermission );

	        return groupPermission;
        }

        public void AddPermission( PermissionDefinition group, string permissionName, ILocalizableString displayName )
        {
	        // create permission
	        PermissionDefinition permission = group.AddChild( permissionName, displayName );

	        // add to permission list
	        PermissionDefinitions.Add( permissionName, permission );
        }

        public void RemoveGroupPermission( PermissionGroupDefinition group, string permissionName )
        {
	        PermissionDefinition groupPermission = group.GetPermissionOrNull( permissionName );

	        if( groupPermission == null )
	        {
		        throw new AbpException( $"Could not find a permission definition with the given name: {permissionName}" );
	        }

	        // remove all child permission
	        foreach( PermissionDefinition childPermission in groupPermission.Children )
	        {
		        PermissionDefinitions.Remove( childPermission.Name );
	        }
	        
	        // remove group
	        PermissionDefinitions.Remove( permissionName );
	        group.RemovePermission( groupPermission );
        }

        public void UpdateGroupPermission( PermissionGroupDefinition group, string permissionName, ILocalizableString displayName )
        {
	        PermissionDefinition groupPermission = group.GetPermissionOrNull( permissionName );

	        if( groupPermission == null )
	        {
		        throw new AbpException( $"Could not find a permission definition with the given name: {permissionName}" );
	        }

	        // update localized name
	        groupPermission.DisplayName = displayName;
	        PermissionDefinitions[permissionName].DisplayName = displayName;
        }

        public void UpdatePermission( PermissionGroupDefinition group, string permissionName, ILocalizableString displayName )
        {
	        PermissionDefinition groupPermission = group.GetPermissionOrNull( permissionName );

	        if( groupPermission == null )
	        {
		        throw new AbpException( $"Could not find a permission definition with the given name: {permissionName}" );
	        }

	        PermissionDefinition childPermission = groupPermission.Children.FirstOrDefault( permission => permission.Name == permissionName );

	        if( childPermission == null )
	        {
		        throw new AbpException( $"Could not find child permission definition with the given name: {displayName}" );
	        }

	        // update localized name
	        childPermission.DisplayName = displayName;
	        PermissionDefinitions[permissionName].DisplayName = displayName;
        }

        protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
        {
            var permissions = new Dictionary<string, PermissionDefinition>();

            foreach (var groupDefinition in PermissionGroupDefinitions.Values)
            {
                foreach (var permission in groupDefinition.Permissions)
                {
                    AddPermissionToDictionaryRecursively(permissions, permission);
                }
            }

            return permissions;
        }

        protected virtual void AddPermissionToDictionaryRecursively(
            Dictionary<string, PermissionDefinition> permissions,
            PermissionDefinition permission)
        {
            if (permissions.ContainsKey(permission.Name))
            {
                throw new AbpException("Duplicate permission name: " + permission.Name);
            }

            permissions[permission.Name] = permission;

            foreach (var child in permission.Children)
            {
                AddPermissionToDictionaryRecursively(permissions, child);
            }
        }

        protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = new PermissionDefinitionContext(scope.ServiceProvider);

                var providers = Options
                        .DefinitionProviders
                        .Select(p => scope.ServiceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)
                        .ToList();

                foreach (var provider in providers)
                {
                    provider.PreDefine(context);
                }

                foreach (var provider in providers)
                {
                    provider.Define(context);
                }

                foreach (var provider in providers)
                {
                    provider.PostDefine(context);
                }

                return context.Groups;
            }
        }
    }
}