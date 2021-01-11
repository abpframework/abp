using System.Collections.Generic;
using System.Collections.Immutable;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.Features
{
    public class FeatureGroupDefinition
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

        public IReadOnlyList<FeatureDefinition> Features => _features.ToImmutableList();
        private readonly List<FeatureDefinition> _features;

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

        protected internal FeatureGroupDefinition(
            string name,
            ILocalizableString displayName = null)
        {
            Name = name;
            DisplayName = displayName ?? new FixedLocalizableString(Name);

            Properties = new Dictionary<string, object>();
            _features = new List<FeatureDefinition>();
        }

        public virtual FeatureDefinition AddFeature(
            string name,
            string defaultValue = null,
            ILocalizableString displayName = null,
            ILocalizableString description = null,
            IStringValueType valueType = null,
            bool isVisibleToClients = true)
        {
            var feature = new FeatureDefinition(
                name,
                defaultValue,
                displayName,
                description,
                valueType,
                isVisibleToClients
            );

            _features.Add(feature);

            return feature;
        }

        public virtual List<FeatureDefinition> GetFeaturesWithChildren()
        {
            var features = new List<FeatureDefinition>();

            foreach (var feature in _features)
            {
                AddFeatureToListRecursively(features, feature);
            }

            return features;
        }

        /// <summary>
        /// Sets a property in the <see cref="Properties"/> dictionary.
        /// This is a shortcut for nested calls on this object.
        /// </summary>
        public virtual FeatureGroupDefinition WithProperty(string key, object value)
        {
            Properties[key] = value;
            return this;
        }

        private void AddFeatureToListRecursively(List<FeatureDefinition> features, FeatureDefinition feature)
        {
            features.Add(feature);

            foreach (var child in feature.Children)
            {
                AddFeatureToListRecursively(features, child);
            }
        }

        public override string ToString()
        {
            return $"[{nameof(FeatureGroupDefinition)} {Name}]";
        }
    }
}
