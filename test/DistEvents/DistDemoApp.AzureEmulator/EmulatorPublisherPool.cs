using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AzureServiceBus;
using Volo.Abp.DependencyInjection;

#nullable enable
namespace DistDemoApp;

public class EmulatorPublisherPool : IPublisherPool, ISingletonDependency
{
    public ILogger<EmulatorPublisherPool> Logger { get; set; }

    private bool _isDisposed;
    private readonly IConnectionPool _connectionPool;
    private readonly ConcurrentDictionary<string, Lazy<ServiceBusSender>> _publishers;

    public EmulatorPublisherPool(IConnectionPool connectionPool)
    {
        _connectionPool = connectionPool;
        _publishers = new ConcurrentDictionary<string, Lazy<ServiceBusSender>>();
        Logger = NullLogger<EmulatorPublisherPool>.Instance;
    }

    public Task<ServiceBusSender> GetAsync(string topicName, string? connectionName)
    {
        var sender = _publishers.GetOrAdd(
            topicName,
            new Lazy<ServiceBusSender>(() =>
            {
                var client = _connectionPool.GetClient(connectionName);
                return client.CreateSender(topicName);
            })
        ).Value;

        return Task.FromResult(sender);
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        if (!_publishers.Any())
        {
            return;
        }

        foreach (var publisher in _publishers.Values)
        {
            await publisher.Value.CloseAsync();
            await publisher.Value.DisposeAsync();
        }

        _publishers.Clear();
    }
}
