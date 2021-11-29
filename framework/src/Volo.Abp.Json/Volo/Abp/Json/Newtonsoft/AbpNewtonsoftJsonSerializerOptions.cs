using Newtonsoft.Json;
using Volo.Abp.Collections;

namespace Volo.Abp.Json.Newtonsoft
{
    public class AbpNewtonsoftJsonSerializerOptions
    {
        public ITypeList<JsonConverter> Converters { get; }

        public AbpNewtonsoftJsonSerializerOptions()
        {
            Converters = new TypeList<JsonConverter>();
        }
    }
}
