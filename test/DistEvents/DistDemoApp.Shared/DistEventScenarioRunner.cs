using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace DistDemoApp;

public class DistEventScenarioRunner : IDistEventScenarioRunner, ITransientDependency
{
    private readonly IDistributedEventBus _distributedEventBus;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public DistEventScenarioRunner(
        IDistributedEventBus distributedEventBus,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _distributedEventBus = distributedEventBus;
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async Task RunAsync(DistEventScenarioProfile profile)
    {
        var typedEventName = EventNameAttribute.GetNameOrDefault<ProviderScenarioEvent>();

        var typedFromTypedPublish = profile.EnableTypedFromTypedScenario
            ? new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously)
            : null;
        var typedFromDynamicPublish = profile.EnableTypedFromDynamicScenario
            ? new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously)
            : null;
        var dynamicOnlyPublish = profile.EnableDynamicOnlyScenario
            ? new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously)
            : null;

        using var typedSubscription = _distributedEventBus.Subscribe<ProviderScenarioEvent>(eventData =>
        {
            if (typedFromTypedPublish != null && eventData.Value == profile.TypedFromTypedValue)
            {
                typedFromTypedPublish.TrySetResult(eventData.Value);
            }

            if (typedFromDynamicPublish != null && eventData.Value == profile.TypedFromDynamicValue)
            {
                typedFromDynamicPublish.TrySetResult(eventData.Value);
            }

            return Task.CompletedTask;
        });

        IDisposable? dynamicOnlySubscription = null;
        if (profile.EnableDynamicOnlyScenario)
        {
            dynamicOnlySubscription = _distributedEventBus.Subscribe(
                profile.DynamicOnlyEventName,
                new SingleInstanceHandlerFactory(
                    new ActionEventHandler<DynamicEventData>(eventData =>
                    {
                        var converted = DynamicEventDataConverter.ConvertToLooseObject(eventData);
                        if (converted is Dictionary<string, object> payload &&
                            payload.TryGetValue("Message", out var message) &&
                            message?.ToString() == profile.DynamicOnlyMessage)
                        {
                            dynamicOnlyPublish!.TrySetResult(true);
                        }

                        return Task.CompletedTask;
                    })));
        }

        await Task.Delay(profile.WarmupDelayMs);

        if (profile.UseUnitOfWork)
        {
            using var uow = _unitOfWorkManager.Begin();
            await PublishScenarioEventsAsync(profile, typedEventName);
            await uow.CompleteAsync();
        }
        else
        {
            await PublishScenarioEventsAsync(profile, typedEventName);
        }

        if (typedFromTypedPublish != null)
        {
            await typedFromTypedPublish.Task.WaitAsync(TimeSpan.FromSeconds(profile.TimeoutSeconds));
        }

        if (typedFromDynamicPublish != null)
        {
            await typedFromDynamicPublish.Task.WaitAsync(TimeSpan.FromSeconds(profile.TimeoutSeconds));
        }

        if (dynamicOnlyPublish != null)
        {
            await dynamicOnlyPublish.Task.WaitAsync(TimeSpan.FromSeconds(profile.TimeoutSeconds));
        }

        dynamicOnlySubscription?.Dispose();

        Console.WriteLine($"All distributed event scenarios passed ({profile.Name}).");
    }

    private async Task PublishScenarioEventsAsync(DistEventScenarioProfile profile, string typedEventName)
    {
        if (profile.EnableTypedFromTypedScenario)
        {
            await _distributedEventBus.PublishAsync(
                new ProviderScenarioEvent { Value = profile.TypedFromTypedValue },
                onUnitOfWorkComplete: profile.OnUnitOfWorkComplete,
                useOutbox: profile.UseOutbox);
        }

        if (profile.EnableTypedFromDynamicScenario)
        {
            await _distributedEventBus.PublishAsync(
                typedEventName,
                new { Value = profile.TypedFromDynamicValue },
                onUnitOfWorkComplete: profile.OnUnitOfWorkComplete,
                useOutbox: profile.UseOutbox);
        }

        if (profile.EnableDynamicOnlyScenario)
        {
            await _distributedEventBus.PublishAsync(
                profile.DynamicOnlyEventName,
                new { Message = profile.DynamicOnlyMessage },
                onUnitOfWorkComplete: profile.OnUnitOfWorkComplete,
                useOutbox: profile.UseOutbox);
        }
    }
}
