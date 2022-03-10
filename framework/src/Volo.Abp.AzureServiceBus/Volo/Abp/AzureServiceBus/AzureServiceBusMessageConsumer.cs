using System;
using System.Collections.Concurrent;
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

    public AzureServiceBusMessageConsumer(
        IExceptionNotifier exceptionNotifier,
        IProcessorPool processorPool)
    {
        _exceptionNotifier = exceptionNotifier;
        _processorPool = processorPool;
        Logger = NullLogger<AzureServiceBusMessageConsumer>.Instance;
        _callbacks = new ConcurrentBag<Func<ServiceBusReceivedMessage, Task>>();
    }

    public virtual void Initialize(
        [NotNull] string topicName,
        [NotNull] string subscriptionName,
        string connectionName)
    {
        Check.NotNull(topicName, nameof(topicName));
        Check.NotNull(subscriptionName, nameof(subscriptionName));

        _topicName = topicName;
        _connectionName = connectionName ?? AzureServiceBusConnections.DefaultConnectionName;
        _subscriptionName = subscriptionName;
        StartProcessing();
    }

    public void OnMessageReceived(Func<ServiceBusReceivedMessage, Task> callback)
    {
        _callbacks.Add(callback);
    }

    protected virtual void StartProcessing()
    {
        Task.Factory.StartNew(function: async () =>
        {
            var serviceBusProcessor = await _processorPool.GetAsync(_subscriptionName, _topicName, _connectionName);
            serviceBusProcessor.ProcessErrorAsync += HandleIncomingError;
            serviceBusProcessor.ProcessMessageAsync += HandleIncomingMessage;

            if (!serviceBusProcessor.IsProcessing)
            {
                await serviceBusProcessor.StartProcessingAsync();
            }

            while (true)
            {
                Thread.Sleep(1000);
            }
        }, TaskCreationOptions.LongRunning);
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
            var serviceBusProcessor = await _processorPool.GetAsync(_subscriptionName, _topicName, _connectionName);
            if(serviceBusProcessor.ReceiveMode == ServiceBusReceiveMode.PeekLock)
            {
                await args.AbandonMessageAsync(args.Message);
            }

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
