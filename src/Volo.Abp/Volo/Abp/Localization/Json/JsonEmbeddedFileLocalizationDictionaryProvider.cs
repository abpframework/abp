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

        public override void Initialize() //TODO: Extract initialization to a factory..?
        {
            var rootNameSpaceWithDot = _rootNamespace + ".";

            var resourceNames = _assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                if (resourceName.StartsWith(rootNameSpaceWithDot))
                {
                    using (var stream = _assembly.GetManifestResourceStream(resourceName))
                    {
                        var jsonString = Utf8Helper.ReadStringFromStream(stream);

                        var dictionary = CreateJsonLocalizationDictionary(jsonString);
                        if (Dictionaries.ContainsKey(dictionary.CultureName))
                        {
                            throw new AbpException($"{resourceName} dictionary has a culture name '{dictionary.CultureName}' which is already defined!");
                        }

                        Dictionaries[dictionary.CultureName] = dictionary;
                    }
                }
            }
        }

        protected virtual ILocalizationDictionary CreateJsonLocalizationDictionary(string jsonString)
        {
            return JsonLocalizationDictionaryBuilder.BuildFromJsonString(jsonString);
        }
    }
}