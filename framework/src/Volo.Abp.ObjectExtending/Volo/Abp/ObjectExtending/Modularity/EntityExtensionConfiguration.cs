using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public class EntityExtensionConfiguration
    {
        [NotNull]
        protected ExtensionPropertyConfigurationDictionary Properties { get; }

        [NotNull]
        public List<Action<ObjectExtensionValidationContext>> Validators { get; }

        public Dictionary<string, object> Configuration { get; }

        public EntityExtensionConfiguration()
        {
            Properties = new ExtensionPropertyConfigurationDictionary();
            Validators = new List<Action<ObjectExtensionValidationContext>>();
            Configuration = new Dictionary<string, object>();
        }

        [NotNull]
        public virtual EntityExtensionConfiguration AddOrUpdateProperty<TProperty>(
            [NotNull] string propertyName,
            [CanBeNull] Action<ExtensionPropertyConfiguration> configureAction = null)
        {
            return AddOrUpdateProperty(
                typeof(TProperty),
                propertyName,
                configureAction
            );
        }

        [NotNull]
        public virtual EntityExtensionConfiguration AddOrUpdateProperty(
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<ExtensionPropertyConfiguration> configureAction = null)
        {
            Check.NotNull(propertyType, nameof(propertyType));
            Check.NotNull(propertyName, nameof(propertyName));

            var propertyInfo = Properties.GetOrAdd(
                propertyName,
                () => new ExtensionPropertyConfiguration(this, propertyType, propertyName)
            );

            configureAction?.Invoke(propertyInfo);

            return this;
        }

        [NotNull]
        public virtual ImmutableList<ExtensionPropertyConfiguration> GetProperties()
        {
            return Properties.Values.ToImmutableList();
        }
    }
}