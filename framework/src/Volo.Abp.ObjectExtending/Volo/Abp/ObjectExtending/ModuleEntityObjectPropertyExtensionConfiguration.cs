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

        [CanBeNull]
        public ILocalizableString DisplayName { get; set; }

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

            Configuration = new Dictionary<object, object>();
            Attributes = new List<Attribute>();
            Validators = new List<Action<ObjectExtensionPropertyValidationContext>>();

            UI = new ModuleEntityObjectPropertyExtensionUIConfiguration();
        }
    }
}