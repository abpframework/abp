using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public interface IPermissionDefinitionManager
{
	[NotNull]
	PermissionDefinition Get([NotNull] string name);

	[CanBeNull]
	PermissionDefinition GetOrNull([NotNull] string name);
        
	[CanBeNull]
	PermissionGroupDefinition GetGroupOrNull([NotNull] string name);
        
	[NotNull]
	PermissionGroupDefinition GetGroup([NotNull] string name);
        
	[NotNull]
	PermissionDefinition Add(
		[NotNull] string name,
		PermissionDefinition parent = null,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySides = MultiTenancySides.Both,
		bool isEnabled = true
	);
		
	[NotNull]
	PermissionDefinition Add(
		[NotNull] string name,
		PermissionGroupDefinition parent = null,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySides = MultiTenancySides.Both,
		bool isEnabled = true
	);

	[NotNull]
	PermissionGroupDefinition AddGroup(
		[NotNull] string name,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySides = MultiTenancySides.Both
	);
        
	void Remove([NotNull] string name, bool removeChildren = true);

	void Remove([NotNull] PermissionDefinition permission, bool removeChildren = true);
        
	void RemoveGroup([NotNull] string name, bool removeChildren = true);

	void RemoveGroup([NotNull] PermissionGroupDefinition permission, bool removeChildren = true);

	void Update(
		[NotNull] string name, 
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySides = MultiTenancySides.Both,
		bool isEnabled = true
	);

	void UpdateGroup(
		[NotNull] string name,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySides = MultiTenancySides.Both
	);

	IReadOnlyList<PermissionDefinition> GetPermissions();

	IReadOnlyList<PermissionGroupDefinition> GetGroups();
}