using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;
using Volo.Abp.Dapr;
using Volo.Abp.EventBus.Dapr;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Controllers;

[Area("abp")]
[RemoteService(Name = "abp")]
public class AbpAspNetCoreMvcDaprEventsController : AbpController
{
    [HttpPost(AbpAspNetCoreMvcDaprPubSubConsts.DaprEventCallbackUrl)]
    public virtual async Task<IActionResult> EventsAsync()
    {
        await HttpContext.ValidateDaprAppApiTokenAsync();

        var bodyJsonDocument = await JsonDocument.ParseAsync(HttpContext.Request.Body);
        var request = JsonSerializer.Deserialize<AbpAspNetCoreMvcDaprSubscriptionRequest>(bodyJsonDocument.RootElement.GetRawText(),
            HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions);

        var distributedEventBus = HttpContext.RequestServices.GetRequiredService<DaprDistributedEventBus>();
        var daprSerializer = HttpContext.RequestServices.GetRequiredService<IDaprSerializer>();

        var eventData = daprSerializer.Deserialize(bodyJsonDocument.RootElement.GetProperty("data").GetRawText(), distributedEventBus.GetEventType(request.Topic));
        await distributedEventBus.TriggerHandlersAsync(distributedEventBus.GetEventType(request.Topic), eventData);

        return Ok();
    }
}
