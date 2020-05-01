using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Localization;

namespace Volo.Abp.ObjectExtending.Modularity
{
    public class ModuleEntityObjectPropertyExtensionConfiguration : IHasNameWithLocalizableDisplayName
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
        public ModuleEntityObjectPropertyExtensionEntityConfiguration Entity { get; }

        [NotNull]
        public ModuleEntityObjectPropertyExtensionUIConfiguration UI { get; }
        
        [NotNull]
        public ModuleEntityObjectPropertyExtensionApiConfiguration Api { get; }

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

            Entity = new ModuleEntityObjectPropertyExtensionEntityConfiguration();
            UI = new ModuleEntityObjectPropertyExtensionUIConfiguration();
            Api = new ModuleEntityObjectPropertyExtensionApiConfiguration();
        }
    }
}