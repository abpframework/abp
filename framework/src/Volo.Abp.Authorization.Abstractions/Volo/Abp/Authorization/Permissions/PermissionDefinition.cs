using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions
{
    public class PermissionDefinition : IHasSimpleStateCheckers<PermissionDefinition>
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

        public List<ISimpleStateChecker<PermissionDefinition>> SimpleStateCheckers { get; }

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
        /// Indicates whether this permission is enabled or disabled.
        /// A permission is normally enabled.
        /// A disabled permission can not be granted to anyone, but it is still
        /// will be available to check its value (while it will always be false).
        ///
        /// Disabling a permission would be helpful to hide a related application
        /// functionality from users/clients.
        ///
        /// Default: true.
        /// </summary>
        public bool IsEnabled { get; set; }

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

        protected internal PermissionDefinition(
            [NotNull] string name,
            ILocalizableString displayName = null,
            MultiTenancySides multiTenancySide = MultiTenancySides.Both,
            bool isEnabled = true)
        {
            Name = Check.NotNull(name, nameof(name));
            DisplayName = displayName ?? new FixedLocalizableString(name);
            MultiTenancySide = multiTenancySide;
            IsEnabled = isEnabled;

            Properties = new Dictionary<string, object>();
            Providers = new List<string>();
            SimpleStateCheckers = new List<ISimpleStateChecker<PermissionDefinition>>();
            _children = new List<PermissionDefinition>();
        }

        public virtual PermissionDefinition AddChild(
            [NotNull] string name,
            ILocalizableString displayName = null,
            MultiTenancySides multiTenancySide = MultiTenancySides.Both,
            bool isEnabled = true)
        {
            var child = new PermissionDefinition(
                name,
                displayName,
                multiTenancySide,
                isEnabled)
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
