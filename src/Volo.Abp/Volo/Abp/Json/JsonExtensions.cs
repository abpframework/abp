using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Volo.Abp.Json
{
    public static class JsonExtensions
    {
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

            //options.Converters.Insert(0, new AbpDateTimeConverter()); //TODO: AbpDateTimeConverter?

            return JsonConvert.SerializeObject(obj, options);
        }
    }
}
