using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionGroupDefinition //TODO: Consider to make possible a group have sub groups
{
	/// <summary>
	/// Unique name of the group.
	/// </summary>
	public string Name { get; }

	public Dictionary<string, object> Properties { get; }

	public ILocalizableString DisplayName
	{
		get => _displayName;
		set => _displayName = Check.NotNull(value, nameof(value));
	}
	private ILocalizableString _displayName;

	/// <summary>
	/// MultiTenancy side.
	/// Default: <see cref="MultiTenancySides.Both"/>
	/// </summary>
	public MultiTenancySides MultiTenancySide { get; set; }

	public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();
	private readonly List<PermissionDefinition> _permissions;

	/// <summary>
	/// Gets/sets a key-value on the <see cref="Properties"/>.
	/// </summary>
	/// <param name="name">Name of the property</param>
	/// <returns>
	/// Returns the value in the <see cref="Properties"/> dictionary by given <paramref name="name"/>.
	/// Returns null if given <paramref name="name"/> is not present in the <see cref="Properties"/> dictionary.
	/// </returns>
	public object this[string name]
	{
		get => Properties.GetOrDefault(name);
		set => Properties[name] = value;
	}

	public PermissionGroupDefinition(
		string name,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySide = MultiTenancySides.Both)
	{
		Name = name;
		DisplayName = displayName ?? new FixedLocalizableString(Name);
		MultiTenancySide = multiTenancySide;

		Properties = new Dictionary<string, object>();
		_permissions = new List<PermissionDefinition>();
	}

	public virtual PermissionDefinition AddPermission(
		string name,
		ILocalizableString displayName = null,
		MultiTenancySides multiTenancySide = MultiTenancySides.Both,
		bool isEnabled = true)
	{
		var permission = new PermissionDefinition(
			name,
			displayName,
			multiTenancySide,
			isEnabled
		);

		_permissions.Add(permission);

		return permission;
	}

	public virtual bool RemovePermission(PermissionDefinition permission)
	{
		return _permissions.Remove(permission);
	}

	public virtual List<PermissionDefinition> GetPermissionsWithChildren()
	{
		var permissions = new List<PermissionDefinition>();

		foreach (var permission in _permissions)
		{
			AddPermissionToListRecursively(permissions, permission);
		}

		return permissions;
	}

	private void AddPermissionToListRecursively(List<PermissionDefinition> permissions, PermissionDefinition permission)
	{
		permissions.Add(permission);

		foreach (var child in permission.Children)
		{
			AddPermissionToListRecursively(permissions, child);
		}
	}

	public override string ToString()
	{
		return $"[{nameof(PermissionGroupDefinition)} {Name}]";
	}

	[CanBeNull]
	public PermissionDefinition GetPermissionOrNull([NotNull] string name)
	{
		Check.NotNull(name, nameof(name));

		return GetPermissionOrNullRecursively(Permissions, name);
	}

	private PermissionDefinition GetPermissionOrNullRecursively(
		IReadOnlyList<PermissionDefinition> permissions, string name)
	{
		foreach (var permission in permissions)
		{
			if (permission.Name == name)
			{
				return permission;
			}

			var childPermission = GetPermissionOrNullRecursively(permission.Children, name);
			if (childPermission != null)
			{
				return childPermission;
			}
		}

		return null;
	}
}