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
        public IDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        [NotNull]
        public List<ILocalizationResourceContributor> Contributors { get; }

        [NotNull]
        public List<Type> BaseResourceTypes { get; }

        public LocalizationResource(
            [NotNull] Type resourceType, 
            [CanBeNull] string defaultCultureName = null,
            [CanBeNull] ILocalizationResourceContributor initialContributor = null)
        {
            ResourceType = Check.NotNull(resourceType, nameof(resourceType));
            DefaultCultureName = defaultCultureName;

            Dictionaries = new Dictionary<string, ILocalizationDictionary>();

            BaseResourceTypes = new List<Type>();
            Contributors = new List<ILocalizationResourceContributor>();

            if (initialContributor != null)
            {
                Contributors.Add(initialContributor);
            }

            AddBaseResourceTypes();
        }

        public virtual void Initialize(IServiceProvider serviceProvider)
        {
            var context = new LocalizationResourceInitializationContext(this, serviceProvider);

            Dictionaries.Clear();
            InitializeContributors(context);

            foreach (var contributor in Contributors)
            {
                contributor.Updated += (sender, args) =>
                {
                    Dictionaries.Clear();
                    InitializeContributors(context);
                };
            }
        }

        protected virtual void InitializeContributors(LocalizationResourceInitializationContext context)
        {
            foreach (var contributor in Contributors)
            {
                foreach (var dictionary in contributor.GetDictionaries(context))
                {
                    var existingDictionary = Dictionaries.GetOrDefault(dictionary.CultureName);
                    if (existingDictionary == null)
                    {
                        Dictionaries[dictionary.CultureName] = dictionary;
                    }
                    else
                    {
                        existingDictionary.Extend(dictionary);
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