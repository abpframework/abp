using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

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

	public PermissionGroupDefinition GetGroup(string name)
	{
		var permission = GetGroupOrNull(name);

		if (permission == null)
		{
			throw new AbpException("Undefined group permission: " + name);
		}

		return permission;
	}

	public PermissionGroupDefinition GetGroupOrNull(string name)
	{
		Check.NotNull(name, nameof(name));

		return PermissionGroupDefinitions.GetOrDefault(name);
	}
        
	public PermissionDefinition Add( 
		string name, 
		PermissionDefinition parent = null, 
		ILocalizableString displayName = null, 
		MultiTenancySides multiTenancySides = MultiTenancySides.Both, 
		bool isEnabled = true )
	{
		if (PermissionDefinitions.ContainsKey(name))
		{
			throw new AbpException($"Permission already exist: {name}");
		}

		var permission = parent?.AddChild( 
			name,
			displayName,
			multiTenancySides,
			isEnabled
		) ?? new PermissionDefinition(
			name,
			displayName,
			multiTenancySides,
			isEnabled
		);
	        
		PermissionDefinitions.Add(name, permission);

		return permission;
	}
        
	public PermissionDefinition Add( 
		string name, 
		PermissionGroupDefinition parent = null, 
		ILocalizableString displayName = null, 
		MultiTenancySides multiTenancySides = MultiTenancySides.Both, 
		bool isEnabled = true )
	{
		if (PermissionDefinitions.ContainsKey(name))
		{
			throw new AbpException($"Permission already exist: {name}");
		}
	        
		var permission = parent?.AddPermission( 
			name,
			displayName,
			multiTenancySides,
			isEnabled
		) ?? new PermissionDefinition(
			name,
			displayName,
			multiTenancySides,
			isEnabled
		);

		PermissionDefinitions.Add(name, permission);

		return permission;
	}

	public PermissionGroupDefinition AddGroup(
		string name,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySides = MultiTenancySides.Both )
	{
		if (PermissionGroupDefinitions.ContainsKey(name))
		{
			throw new AbpException($"Permission group already exist: {name}");
		}

		var permission = new PermissionGroupDefinition(
			name,
			displayName,
			multiTenancySides
		);
	        
		PermissionGroupDefinitions.Add(name, permission);

		return permission;
	}

	public void Remove(string name, bool removeChildren = true)
	{
		var permission = Get(name);
	        
		PermissionDefinitions.Remove(name);

		if (removeChildren)
		{
			foreach (PermissionDefinition childPermission in permission.Children)
			{
				Remove(childPermission);
			}
		}
	        
		permission.Parent?.RemoveChild(permission);
	}

	public void Remove(PermissionDefinition permission, bool removeChildren = true)
	{
		Remove(permission.Name, removeChildren);
	}

	public void RemoveGroup(string name, bool removeChildren = true)
	{
		var permission = GetGroup(name);
	        
		if (removeChildren)
		{
			foreach (PermissionDefinition childPermission in permission.Permissions)
			{
				Remove(childPermission.Name);
			}
		}
	        
		PermissionGroupDefinitions.Remove(name);
	}

	public void RemoveGroup(PermissionGroupDefinition permission, bool removeChildren = true)
	{
		RemoveGroup(permission.Name, removeChildren);
	}

	public void Update(
		string name,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySides = MultiTenancySides.Both,
		bool isEnabled = true)
	{
		var permission = Get(name);

		permission.DisplayName = displayName ?? new FixedLocalizableString(name);
		permission.MultiTenancySide = multiTenancySides;
		permission.IsEnabled = isEnabled;
	}

	public void UpdateGroup(
		string name, 
		ILocalizableString displayName = null, 
		MultiTenancySides multiTenancySides = MultiTenancySides.Both)
	{
		var permission = GetGroup(name);

		permission.DisplayName = displayName ?? new FixedLocalizableString(name);
		permission.MultiTenancySide = multiTenancySides;
	}

	public virtual IReadOnlyList<PermissionDefinition> GetPermissions()
	{
		return PermissionDefinitions.Values.ToImmutableList();
	}

	public IReadOnlyList<PermissionGroupDefinition> GetGroups()
	{
		return PermissionGroupDefinitions.Values.ToImmutableList();
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