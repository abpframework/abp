using Newtonsoft.Json;

namespace Volo.Abp.Json
{
    public class AbpJsonOptions
    {
        public JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings();
    }
}