using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Dapr;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus;

[DependsOn(
    typeof(AbpAspNetCoreMvcDaprModule),
    typeof(AbpEventBusDaprModule)
)]
public class AbpAspNetCoreMvcDaprEventBusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        PostConfigure<AbpEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add(endpointContext =>
            {
                var topicMetadatas = endpointContext.Endpoints.DataSources.SelectMany(x => x.Endpoints).OfType<RouteEndpoint>()
                    .Where(e => e.Metadata.GetOrderedMetadata<ITopicMetadata>().Any(t => t.Name != null))
                    .SelectMany(e => e.Metadata.GetOrderedMetadata<ITopicMetadata>())
                    .ToList();

                var endpointConventionBuilder = endpointContext.Endpoints.MapPost(
                    "/api/abp/dapr/event", async httpContext =>
                    {
                        await HandleEventAsync(httpContext);
                    });

                var abpEvents = GetAbpEvents(endpointContext);
                foreach (var @event in abpEvents.Where(x => !topicMetadatas.Any(t => t.PubsubName == x.PubsubName && t.Name == x.Name)))
                {
                    endpointConventionBuilder.WithMetadata(new TopicAttribute(
                        @event.PubsubName,
                        @event.Name,
                        true));
                }

                endpointContext.Endpoints.MapSubscribeHandler();
            });
        });
    }

    private List<TopicAttribute> GetAbpEvents(EndpointRouteBuilderContext endpointContext)
    {
        var subscriptions = new List<TopicAttribute>();
        var daprEventBusOptions = endpointContext.Endpoints.ServiceProvider.GetRequiredService<IOptions<AbpDaprEventBusOptions>>().Value;

        foreach (var @interface in endpointContext.Endpoints.ServiceProvider.GetRequiredService<IOptions<AbpDistributedEventBusOptions>>().Value.Handlers
                     .SelectMany(x => x.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDistributedEventHandler<>))))
        {
            var eventType = @interface.GetGenericArguments()[0];
            var eventName = EventNameAttribute.GetNameOrDefault(eventType);

            var subscription = new TopicAttribute(daprEventBusOptions.PubSubName, eventName);
            subscriptions.Add(subscription);
        }

        return subscriptions;
    }

    private async static Task HandleEventAsync(HttpContext httpContext)
    {
        var logger = httpContext.RequestServices.GetRequiredService<ILogger<AbpAspNetCoreMvcDaprEventBusModule>>();

        httpContext.ValidateDaprAppApiToken();

        var daprSerializer = httpContext.RequestServices.GetRequiredService<IDaprSerializer>();
        var body = (await JsonDocument.ParseAsync(httpContext.Request.Body));

        var pubSubName = body.RootElement.GetProperty("pubsubname").GetString();
        var topic = body.RootElement.GetProperty("topic").GetString();
        var data = body.RootElement.GetProperty("data").GetRawText();
        if (pubSubName.IsNullOrWhiteSpace() || topic.IsNullOrWhiteSpace() || data.IsNullOrWhiteSpace())
        {
            logger.LogError("Invalid Dapr event request.");
            httpContext.Response.StatusCode = 400;
            return;
        }

        var distributedEventBus = httpContext.RequestServices.GetRequiredService<DaprDistributedEventBus>();

        if (IsAbpDaprEventData(data))
        {
            var daprEventData = daprSerializer.Deserialize(data, typeof(AbpDaprEventData)).As<AbpDaprEventData>();
            var eventType = distributedEventBus.GetEventType(daprEventData.Topic);
            if (eventType != null)
            {
                var eventData = daprSerializer.Deserialize(daprEventData.JsonData, eventType);
                await distributedEventBus.TriggerHandlersAsync(eventType, eventData, daprEventData.MessageId, daprEventData.CorrelationId);
            }
        }
        else
        {
            var eventType = distributedEventBus.GetEventType(topic);
            if (eventType != null)
            {
                var eventData = daprSerializer.Deserialize(data, eventType);
                await distributedEventBus.TriggerHandlersAsync(eventType, eventData);
            }
        }

        httpContext.Response.StatusCode = 200;
    }

    private static bool IsAbpDaprEventData(string data)
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
