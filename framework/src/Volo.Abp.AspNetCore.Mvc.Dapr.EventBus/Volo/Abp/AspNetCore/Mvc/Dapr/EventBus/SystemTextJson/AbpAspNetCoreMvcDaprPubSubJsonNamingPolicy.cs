using System.Text.Json;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.SystemTextJson;

public class AbpAspNetCoreMvcDaprPubSubJsonNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToLower();
    }
}
