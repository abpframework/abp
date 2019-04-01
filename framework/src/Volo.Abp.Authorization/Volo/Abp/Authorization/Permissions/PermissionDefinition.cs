﻿using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionDefinition
    {
        /// <summary>
        /// Unique name of the permission.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Parent of this permission if one exists.
        /// If set, this permission can be granted only if parent is granted.
        /// </summary>
        public PermissionDefinition Parent { get; private set; }

        /// <summary>
        /// MultiTenancy side.
        /// Default: <see cref="MultiTenancySides.Both"/>
        /// </summary>
        public MultiTenancySides MultiTenancySide { get; set; }

        /// <summary>
        /// A list of allowed providers to get/set value of this permission.
        /// An empty list indicates that all providers are allowed.
        /// </summary>
        public List<string> Providers { get; } //TODO: Rename to AllowedProviders?

        public ILocalizableString DisplayName
        {
            get => _displayName;
            set => _displayName = Check.NotNull(value, nameof(value));
        }
        private ILocalizableString _displayName;

        public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();
        private readonly List<PermissionDefinition> _children;

        /// <summary>
        /// Can be used to get/set custom properties for this permission definition.
        /// </summary>
        public Dictionary<string, object> Properties { get; }

        /// <summary>
        /// Gets/sets a key-value on the <see cref="Properties"/>.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>
        /// Returns the value in the <see cref="Properties"/> dictionary by given <see cref="name"/>.
        /// Returns null if given <see cref="name"/> is not present in the <see cref="Properties"/> dictionary.
        /// </returns>
        public object this[string name]
        {
            get => Properties.GetOrDefault(name);
            set => Properties[name] = value;
        }

        protected internal PermissionDefinition(
            [NotNull] string name, 
            ILocalizableString displayName = null,
            MultiTenancySides multiTenancySide = MultiTenancySides.Both)
        {
            Name = Check.NotNull(name, nameof(name));
            DisplayName = displayName ?? new FixedLocalizableString(name);
            MultiTenancySide = multiTenancySide;

            Properties = new Dictionary<string, object>();
            Providers = new List<string>();
            _children = new List<PermissionDefinition>();
        }

        public virtual PermissionDefinition AddChild(
            [NotNull] string name, 
            ILocalizableString displayName = null,
            MultiTenancySides multiTenancySide = MultiTenancySides.Both)
        {
            var child = new PermissionDefinition(
                name, 
                displayName, 
                multiTenancySide)
            {
                Parent = this
            };

            _children.Add(child);

            return child;
        }

        /// <summary>
        /// Sets a property in the <see cref="Properties"/> dictionary.
        /// This is a shortcut for nested calls on this object.
        /// </summary>
        public virtual PermissionDefinition WithProperty(string key, object value)
        {
            Properties[key] = value;
            return this;
        }

        /// <summary>
        /// Sets a property in the <see cref="Properties"/> dictionary.
        /// This is a shortcut for nested calls on this object.
        /// </summary>
        public virtual PermissionDefinition WithProviders(params string[] providers)
        {
            if (!providers.IsNullOrEmpty())
            {
                Providers.AddRange(providers);
            }

            return this;
        }

        public override string ToString()
        {
            return $"[{nameof(PermissionDefinition)} {Name}]";
        }
    }
}