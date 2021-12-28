using System.Text.Json;
using Volo.Abp.Collections;

namespace Volo.Abp.Json.SystemTextJson;

public class AbpSystemTextJsonSerializerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; }

    public ITypeList UnsupportedTypes { get; }

    public AbpSystemTextJsonSerializerOptions()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        UnsupportedTypes = new TypeList();
    }
}
