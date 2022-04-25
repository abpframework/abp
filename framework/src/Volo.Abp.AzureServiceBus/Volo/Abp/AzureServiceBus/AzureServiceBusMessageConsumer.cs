using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Threading;

namespace Volo.Abp.AzureServiceBus;

public class AzureServiceBusMessageConsumer : IAzureServiceBusMessageConsumer, ITransientDependency
{
    public ILogger<AzureServiceBusMessageConsumer> Logger { get; set; }

    private readonly IExceptionNotifier _exceptionNotifier;
    private readonly IProcessorPool _processorPool;
    private readonly ConcurrentBag<Func<ServiceBusReceivedMessage, Task>> _callbacks;
    private string _connectionName;
    private string _subscriptionName;
    private string _topicName;
    private ServiceBusProcessor _serviceBusProcessor;

    public AzureServiceBusMessageConsumer(
        IExceptionNotifier exceptionNotifier,
        IProcessorPool processorPool,
        ILogger<AzureServiceBusMessageConsumer> logger)
    {
        _exceptionNotifier = exceptionNotifier;
        _processorPool = processorPool;
        Logger = logger;
        _callbacks = new ConcurrentBag<Func<ServiceBusReceivedMessage, Task>>();
    }

    public virtual void Initialize(
        [NotNull] string topicName,
        [NotNull] string subscriptionName,
        string connectionName)
    {
        Check.NotNull(topicName, nameof(topicName));
        Check.NotNull(subscriptionName, nameof(subscriptionName));

        Logger.LogDebug($"Initialize AzureServiceBusMessageConsumer Topic {_topicName} subscription {_subscriptionName}");

        _topicName = topicName;
        _connectionName = connectionName ?? AzureServiceBusConnections.DefaultConnectionName;
        _subscriptionName = subscriptionName;
        AsyncHelper.RunSync(()=> StartProcessingAsync());
    }

    public void OnMessageReceived(Func<ServiceBusReceivedMessage, Task> callback)
    {
        _callbacks.Add(callback);
    }

    protected virtual async Task StartProcessingAsync()
    {
        Logger.LogInformation($"Start processing Azure Topic: {_topicName} subscription: {_subscriptionName}");
        _serviceBusProcessor = await _processorPool.GetAsync(_subscriptionName, _topicName, _connectionName);

        if (!_serviceBusProcessor.IsProcessing)
        {
            _serviceBusProcessor.ProcessErrorAsync += HandleIncomingError;
            _serviceBusProcessor.ProcessMessageAsync += HandleIncomingMessage;

            await _serviceBusProcessor.StartProcessingAsync();
        }
        else
        {
            Logger.LogDebug($"ServiceBusProcessor for Azure Topic {_topicName} subscription {_subscriptionName} is already processing");
        }
    }

    public virtual async Task StopProcessingAsync()
    {
        Logger.LogDebug($"Stop processing Azure Topic {_topicName} subscription {_subscriptionName}");
        if (_serviceBusProcessor.IsProcessing)
            await _serviceBusProcessor.StartProcessingAsync();

            _serviceBusProcessor.ProcessErrorAsync -= HandleIncomingError;
            _serviceBusProcessor.ProcessMessageAsync -= HandleIncomingMessage;

    }
    protected virtual async Task HandleIncomingMessage(ProcessMessageEventArgs args)
    {
        try
        {
            foreach (var callback in _callbacks)
            {
                await callback(args.Message);
            }

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception exception)
        {
            await HandleError(exception);
        }
    }

    protected virtual async Task HandleIncomingError(ProcessErrorEventArgs args)
    {
        await HandleError(args.Exception);
    }

    protected virtual async Task HandleError(Exception exception)
    {
        Logger.LogException(exception);
        await _exceptionNotifier.NotifyAsync(exception);
    }
}
