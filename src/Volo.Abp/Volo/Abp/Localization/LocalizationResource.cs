using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Localization
{
    public class LocalizationResource
    {
        public Type ResourceType { get; }

        public string DefaultCultureName { get; set; }

        public ILocalizationDictionaryProvider DictionaryProvider { get; }

        public List<ILocalizationDictionaryProvider> Extensions { get; }

        public List<Type> BaseResourceTypes { get; }

        public LocalizationResource([NotNull] Type resourceType, [NotNull] string defaultCultureName, [NotNull] ILocalizationDictionaryProvider dictionaryProvider)
        {
            Check.NotNull(resourceType, nameof(resourceType));
            Check.NotNull(defaultCultureName, nameof(defaultCultureName));
            Check.NotNull(dictionaryProvider, nameof(dictionaryProvider));

            ResourceType = resourceType;
            DefaultCultureName = defaultCultureName;
            DictionaryProvider = dictionaryProvider;

            BaseResourceTypes = new List<Type>();
            Extensions = new List<ILocalizationDictionaryProvider>();

            AddBaseResourceTypes();
        }

        public virtual void Initialize(IServiceProvider serviceProvider) //TODO: Create a LocalizationResourceInitializationContext!
        {
            DictionaryProvider.Initialize();

            foreach (var extension in Extensions)
            {
                extension.Initialize();
                DictionaryProvider.Extend(extension);
            }
        }

        protected virtual void AddBaseResourceTypes()
        {
            var descriptors = ResourceType
                .GetCustomAttributes(true)
                .OfType<IInheritedResourceTypesProvider>();

            foreach (var descriptor in descriptors)
            {
                foreach (var baseResourceType in descriptor.GetInheritedModuleTypes())
                {
                    BaseResourceTypes.AddIfNotContains(baseResourceType);
                }
            }
        }
    }
}