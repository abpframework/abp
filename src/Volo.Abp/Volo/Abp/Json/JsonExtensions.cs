using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Volo.Abp.Json
{
    public static class JsonExtensions
    {
        //TODO: Remove this extension method, create IJsonSerializer abstraction (if there is not already such an abstraction).

        /// <summary>
        /// Converts given object to JSON string.
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
        {
            var options = new JsonSerializerSettings();

            if (camelCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented)
            {
                options.Formatting = Formatting.Indented;
            }

            //TODO: AbpDateTimeConverter contains Clock, so it should be injected!
            //options.Converters.Insert(0, new AbpDateTimeConverter());

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}
