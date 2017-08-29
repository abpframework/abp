using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Json.Newtonsoft
{
    public class NewtonsoftJsonSerializer : IJsonSerializer, ITransientDependency
    {
        private readonly AbpJsonIsoDateTimeConverter _dateTimeConverter;

        public NewtonsoftJsonSerializer(AbpJsonIsoDateTimeConverter dateTimeConverter)
        {
            _dateTimeConverter = dateTimeConverter;
        }

        public string Serialize(object obj, bool camelCase = false, bool indented = false)
        {
            var serializerSettings = CreateDefaultSerializerSettings();
            
            if (camelCase)
            {
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented)
            {
                serializerSettings.Formatting = Formatting.Indented;
            }

            return JsonConvert.SerializeObject(obj, serializerSettings);
        }

        protected virtual JsonSerializerSettings CreateDefaultSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Insert(0, _dateTimeConverter);
            return settings;
        }

        public T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}