using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.ObjectExtending
{
    public class ModuleEntityObjectPropertyExtensionConfiguration
    {
        [NotNull]
        public ModuleEntityObjectExtensionConfiguration EntityObjectExtensionConfiguration { get; }

        [NotNull]
        public string Name { get; }

        [NotNull]
        public Type Type { get; }

        [NotNull]
        public List<Attribute> Attributes { get; }

        [NotNull]
        public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

        [NotNull]
        public ILocalizableString DisplayName
        {
            get => _displayName;
            set
            {
                Check.NotNull(value, nameof(value));
                _displayName = value;
            }
        }
        private ILocalizableString _displayName;

        [NotNull]
        public Dictionary<object, object> Configuration { get; }
        
        [NotNull]
        public ModuleEntityObjectPropertyExtensionUIConfiguration UI { get; }

        public ModuleEntityObjectPropertyExtensionConfiguration(
            [NotNull] ModuleEntityObjectExtensionConfiguration entityObjectExtensionConfiguration,
            [NotNull] Type type,
            [NotNull] string name)
        {
            EntityObjectExtensionConfiguration = Check.NotNull(entityObjectExtensionConfiguration, nameof(entityObjectExtensionConfiguration));
            Type = Check.NotNull(type, nameof(type));
            Name = Check.NotNull(name, nameof(name));

            DisplayName = new FixedLocalizableString(Name);

            Configuration = new Dictionary<object, object>();
            Attributes = new List<Attribute>();
            Validators = new List<Action<ObjectExtensionPropertyValidationContext>>();

            UI = new ModuleEntityObjectPropertyExtensionUIConfiguration();
        }
    }
}