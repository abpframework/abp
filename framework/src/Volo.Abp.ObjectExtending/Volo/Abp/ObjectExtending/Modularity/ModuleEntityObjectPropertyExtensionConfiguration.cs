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

        /// <summary>
        /// Single point to enable/disable this property for the clients (UI and API).
        /// If this is false, the configuration made in the <see cref="UI"/> and the <see cref="Api"/>
        /// properties are not used.
        /// Default: true.
        /// </summary>
        public bool IsAvailableToClients { get; set; } = true;

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