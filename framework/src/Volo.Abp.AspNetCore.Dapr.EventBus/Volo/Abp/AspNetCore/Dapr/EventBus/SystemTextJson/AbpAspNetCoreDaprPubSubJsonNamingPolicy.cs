using System.Text.Json;

namespace Volo.Abp.AspNetCore.Dapr.SystemTextJson;

public class AbpAspNetCoreDaprPubSubJsonNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToLower();
    }
}
