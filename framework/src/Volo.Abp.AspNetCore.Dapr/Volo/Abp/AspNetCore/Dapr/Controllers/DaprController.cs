using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Dapr.Models;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Dapr;
using Volo.Abp.EventBus.Dapr;

namespace Volo.Abp.AspNetCore.Dapr.Controllers;

[Area("abp")]
[RemoteService(Name = "abp")]
public class DaprController : AbpController
{
    protected AbpDaprPubSubProvider DaprPubSubProvider { get; }

    public DaprController(AbpDaprPubSubProvider daprPubSubProvider)
    {
        DaprPubSubProvider = daprPubSubProvider;
    }

    [HttpGet(AbpAspNetCoreDaprConsts.DaprSubscribeUrl)]
    public virtual async Task<List<DaprSubscriptionDefinition>> SubscribeAsync()
    {
        return await DaprPubSubProvider.GetSubscriptionsAsync();
    }

    [HttpPost(AbpAspNetCoreDaprConsts.DaprEventCallbackUrl)]
    public virtual async Task<IActionResult> EventsAsync()
    {
        var bodyJsonDocument = await JsonDocument.ParseAsync(HttpContext.Request.Body);
        var request = JsonSerializer.Deserialize<DaprSubscriptionRequest>(bodyJsonDocument.RootElement.GetRawText(),
            HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions);

        var distributedEventBus = HttpContext.RequestServices.GetRequiredService<DaprDistributedEventBus>();
        var daprSerializer = HttpContext.RequestServices.GetRequiredService<IDaprSerializer>();

        var eventData = daprSerializer.Deserialize(bodyJsonDocument.RootElement.GetProperty("data").GetRawText(), distributedEventBus.GetEventType(request.Topic));
        await distributedEventBus.TriggerHandlersAsync(distributedEventBus.GetEventType(request.Topic), eventData);

        return Ok();
    }
}
