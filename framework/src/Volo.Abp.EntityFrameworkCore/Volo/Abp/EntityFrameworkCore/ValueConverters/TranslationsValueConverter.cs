using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Volo.Abp.Data;
using Volo.Abp.Json.SystemTextJson.JsonConverters;


namespace Volo.Abp.EntityFrameworkCore.ValueConverters
{
    public class TranslationsValueConverter : ValueConverter<TranslationDictionary, string>
    {
        public TranslationsValueConverter()
            : base(
                d => SerializeObject(d),
                s => DeserializeObject(s))
        {
        }

        private static string SerializeObject(TranslationDictionary translations)
        {
            return JsonSerializer.Serialize(translations);
        }

        private static TranslationDictionary DeserializeObject(string translationsAsJson)
        {
            if (translationsAsJson.IsNullOrEmpty() || translationsAsJson == "{}")
            {
                return new TranslationDictionary();
            }

            var deserializeOptions = new JsonSerializerOptions();
            deserializeOptions.Converters.Add(new ObjectToInferredTypesConverter());
            var dictionary =
                JsonSerializer.Deserialize<TranslationDictionary>(translationsAsJson, deserializeOptions) ??
                new TranslationDictionary();

            return dictionary;
        }
    }
}