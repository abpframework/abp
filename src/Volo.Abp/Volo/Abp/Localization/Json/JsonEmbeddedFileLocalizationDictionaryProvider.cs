using System.Reflection;
using Volo.Abp.Internal;

namespace Volo.Abp.Localization.Json
{
    /// <summary>
    /// Provides localization dictionaries from JSON files embedded into an <see cref="Assembly"/>.
    /// </summary>
    public class JsonEmbeddedFileLocalizationDictionaryProvider : LocalizationDictionaryProviderBase
    {
        private readonly Assembly _assembly;
        private readonly string _rootNamespace;

        public JsonEmbeddedFileLocalizationDictionaryProvider(Assembly assembly, string rootNamespace)
        {
            _assembly = assembly;
            _rootNamespace = rootNamespace;
        }

        public override void Initialize(LocalizationResource resource)
        {
            var resourceNames = _assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                if (resourceName.StartsWith(_rootNamespace))
                {
                    using (var stream = _assembly.GetManifestResourceStream(resourceName))
                    {
                        var jsonString = Utf8Helper.ReadStringFromStream(stream);

                        var dictionary = CreateJsonLocalizationDictionary(jsonString);
                        if (Dictionaries.ContainsKey(dictionary.CultureName))
                        {
                            throw new AbpException(resource.ResourceType.FullName + " source contains more than one dictionary for the culture: " + dictionary.CultureName);
                        }

                        Dictionaries[dictionary.CultureName] = dictionary;
                    }
                }
            }
        }

        protected virtual JsonLocalizationDictionary CreateJsonLocalizationDictionary(string jsonString)
        {
            return JsonLocalizationDictionary.BuildFromJsonString(jsonString);
        }
    }
}