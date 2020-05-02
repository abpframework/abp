using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public class ModuleEntityObjectExtensionConfiguration
    {
        [NotNull]
        protected ModuleEntityObjectPropertyExtensionConfigurationDictionary Properties { get; }

        [NotNull]
        public List<Action<ObjectExtensionValidationContext>> Validators { get; }

        public ModuleEntityObjectExtensionConfiguration()
        {
            Properties = new ModuleEntityObjectPropertyExtensionConfigurationDictionary();
            Validators = new List<Action<ObjectExtensionValidationContext>>();
        }

        [NotNull]
        public virtual ModuleEntityObjectExtensionConfiguration AddOrUpdateProperty<TProperty>(
            [NotNull] string propertyName,
            [CanBeNull] Action<ModuleEntityObjectPropertyExtensionConfiguration> configureAction = null)
        {
            return AddOrUpdateProperty(
                typeof(TProperty),
                propertyName,
                configureAction
            );
        }

        [NotNull]
        public virtual ModuleEntityObjectExtensionConfiguration AddOrUpdateProperty(
            [NotNull] Type propertyType,
            [NotNull] string propertyName,
            [CanBeNull] Action<ModuleEntityObjectPropertyExtensionConfiguration> configureAction = null)
        {
            Check.NotNull(propertyType, nameof(propertyType));
            Check.NotNull(propertyName, nameof(propertyName));

            var propertyInfo = Properties.GetOrAdd(
                propertyName,
                () => new ModuleEntityObjectPropertyExtensionConfiguration(this, propertyType, propertyName)
            );

            configureAction?.Invoke(propertyInfo);

            return this;
        }

        [NotNull]
        public virtual ImmutableList<ModuleEntityObjectPropertyExtensionConfiguration> GetProperties()
        {
            return Properties.Values.ToImmutableList();
        }
    }
}