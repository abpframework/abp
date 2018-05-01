using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.Localization
{
    public class LocalizationResource
    {
        [NotNull]
        public Type ResourceType { get; }

        [CanBeNull]
        public string DefaultCultureName { get; set; }

        [NotNull]
        public ILocalizationDictionaryProvider DictionaryProvider { get; }

        [NotNull]
        public List<ILocalizationDictionaryProvider> Extensions { get; }

        [NotNull]
        public List<Type> BaseResourceTypes { get; }

        public LocalizationResource(
            [NotNull] Type resourceType, 
            [CanBeNull] string defaultCultureName, //TODO: defaultCultureName should be optional (and second argument) because it's not required for the LocalizationResource!
            [NotNull] ILocalizationDictionaryProvider dictionaryProvider)
        {
            ResourceType = Check.NotNull(resourceType, nameof(resourceType));
            DictionaryProvider = Check.NotNull(dictionaryProvider, nameof(dictionaryProvider));
            DefaultCultureName = defaultCultureName;

            BaseResourceTypes = new List<Type>();
            Extensions = new List<ILocalizationDictionaryProvider>();

            AddBaseResourceTypes();
        }

        public virtual void Initialize(IServiceProvider serviceProvider)
        {
            //TODO: We should refactor here to create a better design!

            var context = new LocalizationResourceInitializationContext(serviceProvider);

            InitializeDictionaryProvider(context);
            InitializeExtensions(context);

            DictionaryProvider.Updated += (sender, args) =>
            {
                InitializeExtensions(context);
            };

            foreach (var extension in Extensions)
            {
                extension.Updated += (sender, args) =>
                {
                    InitializeDictionaryProvider(context);
                    InitializeExtensions(context);
                };
            }
        }

        private void InitializeExtensions(LocalizationResourceInitializationContext context)
        {
            foreach (var extension in Extensions)
            {
                extension.Initialize(context);
                DictionaryProvider.Extend(extension);
            }
        }

        private void InitializeDictionaryProvider(LocalizationResourceInitializationContext context)
        {
            DictionaryProvider.Initialize(context);
        }

        protected virtual void AddBaseResourceTypes()
        {
            var descriptors = ResourceType
                .GetCustomAttributes(true)
                .OfType<IInheritedResourceTypesProvider>();

            foreach (var descriptor in descriptors)
            {
                foreach (var baseResourceType in descriptor.GetInheritedResourceTypes())
                {
                    BaseResourceTypes.AddIfNotContains(baseResourceType);
                }
            }
        }
    }
}