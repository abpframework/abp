using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        var body = (await JsonDocument.ParseAsync(HttpContext.Request.Body));

        var pubSubName = body.RootElement.GetProperty("pubsubname").GetString();
        var topic = body.RootElement.GetProperty("topic").GetString();
        var data = body.RootElement.GetProperty("data").GetRawText();
        if (pubSubName.IsNullOrWhiteSpace() || topic.IsNullOrWhiteSpace() || data.IsNullOrWhiteSpace())
        {
            Logger.LogError("Invalid Dapr event request.");
            return BadRequest();
        }

        var distributedEventBus = HttpContext.RequestServices.GetRequiredService<DaprDistributedEventBus>();

        if (IsAbpDaprEventData(data))
        {
            var daprEventData = daprSerializer.Deserialize(data, typeof(AbpDaprEventData)).As<AbpDaprEventData>();
            var eventData = daprSerializer.Deserialize(daprEventData.JsonData, distributedEventBus.GetEventType(daprEventData.Topic));
            await distributedEventBus.TriggerHandlersAsync(distributedEventBus.GetEventType(daprEventData.Topic), eventData, daprEventData.MessageId, daprEventData.CorrelationId);
        }
        else
        {
            var eventData = daprSerializer.Deserialize(data, distributedEventBus.GetEventType(topic!));
            await distributedEventBus.TriggerHandlersAsync(distributedEventBus.GetEventType(topic!), eventData);
        }

        return Ok();
    }

    protected  virtual bool IsAbpDaprEventData(string data)
    {
        var document = JsonDocument.Parse(data);
        var objects = document.RootElement.EnumerateObject().ToList();
        return objects.Count == 5 &&
               objects.Any(x => x.Name.Equals("PubSubName", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("Topic", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("MessageId", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("JsonData", StringComparison.CurrentCultureIgnoreCase)) &&
               objects.Any(x => x.Name.Equals("CorrelationId", StringComparison.CurrentCultureIgnoreCase));
    }
}
