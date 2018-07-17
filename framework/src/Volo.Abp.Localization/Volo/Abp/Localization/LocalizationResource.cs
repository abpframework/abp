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

        [NotNull]
        public string ResourceName => LocalizationResourceNameAttribute.GetName(ResourceType);

        [CanBeNull]
        public string DefaultCultureName { get; set; }

        [NotNull]
        public IReadOnlyDictionary<string, ILocalizationDictionary> Dictionaries => _dictionaries;
        private readonly Dictionary<string, ILocalizationDictionary> _dictionaries;

        [NotNull]
        public List<ILocalizationResourceContributor> Contributors { get; }

        [NotNull]
        public List<Type> BaseResourceTypes { get; }

        public EventHandler Updated;

        internal bool RegisteredToUpdate { get; set; }

        public LocalizationResource(
            [NotNull] Type resourceType, 
            [CanBeNull] string defaultCultureName = null,
            [CanBeNull] ILocalizationResourceContributor initialContributor = null)
        {
            ResourceType = Check.NotNull(resourceType, nameof(resourceType));
            DefaultCultureName = defaultCultureName;

            _dictionaries = new Dictionary<string, ILocalizationDictionary>();

            BaseResourceTypes = new List<Type>();
            Contributors = new List<ILocalizationResourceContributor>();

            if (initialContributor != null)
            {
                Contributors.Add(initialContributor);
            }

            AddBaseResourceTypes();
        }

        public virtual void FillDictionaries(IServiceProvider serviceProvider)
        {
            _dictionaries.Clear();

            var context = new LocalizationResourceInitializationContext(this, serviceProvider);

            foreach (var contributor in Contributors)
            {
                foreach (var dictionary in contributor.GetDictionaries(context))
                {
                    var existingDictionary = _dictionaries.GetOrDefault(dictionary.CultureName);
                    if (existingDictionary == null)
                    {
                        _dictionaries[dictionary.CultureName] = new CombinedLocalizationDictionary(dictionary);
                    }
                    else
                    {
                        _dictionaries[dictionary.CultureName].As<CombinedLocalizationDictionary>().AddFirst(dictionary);
                    }
                }
            }
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