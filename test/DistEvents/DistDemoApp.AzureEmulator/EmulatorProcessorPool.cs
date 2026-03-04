using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AzureServiceBus;
using Volo.Abp.DependencyInjection;

namespace DistDemoApp;

public class EmulatorProcessorPool : IProcessorPool, ISingletonDependency
{
    public ILogger<EmulatorProcessorPool> Logger { get; set; }

    private bool _isDisposed;
    private readonly AbpAzureServiceBusOptions _options;
    private readonly IConnectionPool _connectionPool;
    private readonly ConcurrentDictionary<string, Lazy<ServiceBusProcessor>> _processors;

    public EmulatorProcessorPool(
        IOptions<AbpAzureServiceBusOptions> options,
        IConnectionPool connectionPool)
    {
        _options = options.Value;
        _connectionPool = connectionPool;
        _processors = new ConcurrentDictionary<string, Lazy<ServiceBusProcessor>>();
        Logger = NullLogger<EmulatorProcessorPool>.Instance;
    }

    public Task<ServiceBusProcessor> GetAsync(string subscriptionName, string topicName, string connectionName)
    {
        var processor = _processors.GetOrAdd(
            $"{topicName}-{subscriptionName}",
            new Lazy<ServiceBusProcessor>(() =>
            {
                var config = _options.Connections.GetOrDefault(connectionName);
                var client = _connectionPool.GetClient(connectionName);
                return client.CreateProcessor(topicName, subscriptionName, config.Processor);
            })
        ).Value;

        return Task.FromResult(processor);
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        if (!_processors.Any())
        {
            return;
        }

        foreach (var item in _processors.Values)
        {
            var processor = item.Value;
            if (processor.IsProcessing)
            {
                await processor.StopProcessingAsync();
            }

            if (!processor.IsClosed)
            {
                await processor.CloseAsync();
            }

            await processor.DisposeAsync();
        }

        _processors.Clear();
    }
}
