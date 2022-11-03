using System.Text.Json;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Json;

public class AbpDaprSubscriptionRequestJsonNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToLower();
    }
}
