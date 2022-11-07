using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Json;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;
using Volo.Abp.Dapr;
using Volo.Abp.EventBus.Dapr;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Controllers;

[Area("abp")]
[RemoteService(Name = "abp")]
public class AbpAspNetCoreMvcDaprEventsController : AbpController
{
    [HttpPost(AbpAspNetCoreMvcDaprPubSubConsts.DaprEventCallbackUrl)]
    public virtual async Task<IActionResult> EventAsync()
    {
        HttpContext.ValidateDaprAppApiToken();

        var daprSerializer = HttpContext.RequestServices.GetRequiredService<IDaprSerializer>();
        var request = (await JsonDocument.ParseAsync(HttpContext.Request.Body)).Deserialize<AbpDaprSubscriptionRequest<object>>(CreateJsonSerializerOptions(daprSerializer));
        if (request != null && request.Data is JsonElement jsonElement)
        {
            var distributedEventBus = HttpContext.RequestServices.GetRequiredService<DaprDistributedEventBus>();
            var eventData = daprSerializer.Deserialize(jsonElement.GetRawText(), distributedEventBus.GetEventType(request.Topic));
            await distributedEventBus.TriggerHandlersAsync(distributedEventBus.GetEventType(request.Topic), eventData);
            return Ok();
        }

        Logger.LogError("Invalid Dapr event request.");
        return BadRequest();
    }

    private static readonly ConcurrentDictionary<string, JsonSerializerOptions> JsonSerializerOptionsCache = new ConcurrentDictionary<string, JsonSerializerOptions>();

    protected virtual JsonSerializerOptions CreateJsonSerializerOptions(IDaprSerializer daprSerializer)
    {
        return JsonSerializerOptionsCache.GetOrAdd(nameof(AbpAspNetCoreMvcDaprEventsController), _ =>
        {
            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = new AbpDaprSubscriptionRequestJsonNamingPolicy()
            };
            settings.Converters.Add(new AbpDaprSubscriptionRequestConverterFactory(daprSerializer));
            return settings;
        });
    }
}
