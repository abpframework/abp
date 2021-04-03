using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AzureServiceBus
{
    public class PublisherPool : IPublisherPool, ISingletonDependency
    {
        public ILogger<PublisherPool> Logger { get; set; }

        private bool _isDisposed;
        private readonly IConnectionPool _connectionPool;
        private readonly ConcurrentDictionary<string, Lazy<ServiceBusSender>> _publishers;

        public PublisherPool(IConnectionPool connectionPool)
        {
            _connectionPool = connectionPool;
            _publishers = new ConcurrentDictionary<string, Lazy<ServiceBusSender>>();
            Logger = new NullLogger<PublisherPool>();
        }

        public async Task<ServiceBusSender> GetAsync(string topicName, string connectionName)
        {
            var admin = _connectionPool.GetAdministrationClient(connectionName);
            await admin.SetupTopicAsync(topicName);

            return _publishers.GetOrAdd(
                topicName, new Lazy<ServiceBusSender>(() =>
                {
                    var client = _connectionPool.GetClient(connectionName);
                    return client.CreateSender(topicName);
                })
            ).Value;
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
                Logger.LogDebug($"Disposed publisher pool with no publisher in the pool.");
                return;
            }

            Logger.LogInformation($"Disposing publisher pool ({_publishers.Count} publishers).");

            foreach (var publisher in _publishers.Values)
            {
                await publisher.Value.CloseAsync();
                await publisher.Value.DisposeAsync();
            }

            _publishers.Clear();
        }
    }
}