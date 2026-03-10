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
        var typedFromAnonymousPublish = profile.EnableTypedFromAnonymousScenario
            ? new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously)
            : null;
        var anonymousOnlyPublish = profile.EnableAnonymousOnlyScenario
            ? new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously)
            : null;

        using var typedSubscription = _distributedEventBus.Subscribe<ProviderScenarioEvent>(eventData =>
        {
            if (typedFromTypedPublish != null && eventData.Value == profile.TypedFromTypedValue)
            {
                typedFromTypedPublish.TrySetResult(eventData.Value);
            }

            if (typedFromAnonymousPublish != null && eventData.Value == profile.TypedFromAnonymousValue)
            {
                typedFromAnonymousPublish.TrySetResult(eventData.Value);
            }

            return Task.CompletedTask;
        });

        IDisposable? anonymousOnlySubscription = null;
        if (profile.EnableAnonymousOnlyScenario)
        {
            anonymousOnlySubscription = _distributedEventBus.Subscribe(
                profile.AnonymousOnlyEventName,
                new SingleInstanceHandlerFactory(
                    new ActionEventHandler<AnonymousEventData>(eventData =>
                    {
                        var converted = AnonymousEventDataConverter.ConvertToLooseObject(eventData);
                        if (converted is Dictionary<string, object> payload &&
                            payload.TryGetValue("Message", out var message) &&
                            message?.ToString() == profile.AnonymousOnlyMessage)
                        {
                            anonymousOnlyPublish!.TrySetResult(true);
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

        if (typedFromAnonymousPublish != null)
        {
            await typedFromAnonymousPublish.Task.WaitAsync(TimeSpan.FromSeconds(profile.TimeoutSeconds));
        }

        if (anonymousOnlyPublish != null)
        {
            await anonymousOnlyPublish.Task.WaitAsync(TimeSpan.FromSeconds(profile.TimeoutSeconds));
        }

        anonymousOnlySubscription?.Dispose();

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

        if (profile.EnableTypedFromAnonymousScenario)
        {
            await _distributedEventBus.PublishAsync(
                typedEventName,
                new { Value = profile.TypedFromAnonymousValue },
                onUnitOfWorkComplete: profile.OnUnitOfWorkComplete,
                useOutbox: profile.UseOutbox);
        }

        if (profile.EnableAnonymousOnlyScenario)
        {
            await _distributedEventBus.PublishAsync(
                profile.AnonymousOnlyEventName,
                new { Message = profile.AnonymousOnlyMessage },
                onUnitOfWorkComplete: profile.OnUnitOfWorkComplete,
                useOutbox: profile.UseOutbox);
        }
    }
}
