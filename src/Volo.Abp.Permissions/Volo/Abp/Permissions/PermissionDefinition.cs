using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.Permissions
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

        public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();
        private readonly List<PermissionDefinition> _children;

        public Dictionary<string, object> Properties { get; set; }

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

        protected internal PermissionDefinition(string name)
        {
            Name = name;
            _children = new List<PermissionDefinition>();
        }

        public virtual PermissionDefinition AddChild(string name)
        {
            var child = new PermissionDefinition(name)
            {
                Parent = this
            };

            _children.Add(child);

            return child;
        }

        public override string ToString()
        {
            return $"[Permission {Name}]";
        }
    }
}