using Newtonsoft.Json;

namespace Volo.Abp.Json.Newtonsoft;

public class AbpNewtonsoftJsonSerializerOptions
{
    public JsonSerializerSettings JsonSerializerSettings { get; }

    public AbpNewtonsoftJsonSerializerOptions()
    {
        JsonSerializerSettings = new JsonSerializerSettings();
    }
}
