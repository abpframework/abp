using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.Newtonsoft
{
    public class NewtonsoftJsonSerializer : IJsonSerializer, ITransientDependency
    {
        private readonly AbpJsonIsoDateTimeConverter _dateTimeConverter;
        private readonly AbpJsonOptions _abpJsonOptions;

        private static readonly CamelCaseExceptDictionaryKeysResolver SharedCamelCaseExceptDictionaryKeysResolver =
            new CamelCaseExceptDictionaryKeysResolver();

        public NewtonsoftJsonSerializer(AbpJsonIsoDateTimeConverter dateTimeConverter, IOptions<AbpJsonOptions> abpJsonOptions)
        {
            _dateTimeConverter = dateTimeConverter;
            _abpJsonOptions = abpJsonOptions.Value;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            return JsonConvert.SerializeObject(obj, CreateSerializerSettings(camelCase, indented));
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, CreateSerializerSettings(camelCase));
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            return JsonConvert.DeserializeObject(jsonString, type, CreateSerializerSettings(camelCase));
        }

        protected virtual JsonSerializerSettings CreateSerializerSettings(bool camelCase = true, bool indented = false)
        {
            var settings = _abpJsonOptions.SerializerSettings;

            settings.Converters.Insert(0, _dateTimeConverter);
            
            if (camelCase)
            {
                settings.ContractResolver = SharedCamelCaseExceptDictionaryKeysResolver;
            }

            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }
            
            return settings;
        }

        private class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
        {
            protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
            {
                var contract = base.CreateDictionaryContract(objectType);

                contract.DictionaryKeyResolver = propertyName => propertyName;

                return contract;
            }
        }
    }
}