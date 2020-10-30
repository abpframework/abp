using System.Text.Json;

namespace Volo.Abp.Json.SystemTextJson
{
    public class AbpSystemTextJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; }

        public AbpSystemTextJsonSerializerOptions()
        {
            //TODO:Defaults?
            //https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.Core/src/JsonOptions.cs#L18
            //https://github.com/dotnet/runtime/blob/master/src/libraries/System.Text.Json/src/System/Text/Json/Serialization/JsonSerializerDefaults.cs

            JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }
    }
}
