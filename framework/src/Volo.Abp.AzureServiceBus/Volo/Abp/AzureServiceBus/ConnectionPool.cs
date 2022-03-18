using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AzureServiceBus;

public class ConnectionPool : IConnectionPool, ISingletonDependency
{
    public ILogger<ConnectionPool> Logger { get; set; }

    private bool _isDisposed;
    private readonly AbpAzureServiceBusOptions _options;
    private readonly ConcurrentDictionary<string, Lazy<ServiceBusClient>> _clients;
    private readonly ConcurrentDictionary<string, Lazy<ServiceBusAdministrationClient>> _adminClients;

    public ConnectionPool(IOptions<AbpAzureServiceBusOptions> options)
    {
        _options = options.Value;
        _clients = new ConcurrentDictionary<string, Lazy<ServiceBusClient>>();
        _adminClients = new ConcurrentDictionary<string, Lazy<ServiceBusAdministrationClient>>();
        Logger = new NullLogger<ConnectionPool>();
    }

    public ServiceBusClient GetClient(string connectionName)
    {
        connectionName ??= AzureServiceBusConnections.DefaultConnectionName;
        return _clients.GetOrAdd(
            connectionName, new Lazy<ServiceBusClient>(() =>
            {
                var config = _options.Connections.GetOrDefault(connectionName);
                return new ServiceBusClient(config.ConnectionString, config.Client);
            })
        ).Value;
    }

    public ServiceBusAdministrationClient GetAdministrationClient(string connectionName)
    {
        connectionName ??= AzureServiceBusConnections.DefaultConnectionName;
        return _adminClients.GetOrAdd(
            connectionName, new Lazy<ServiceBusAdministrationClient>(() =>
            {
                var config = _options.Connections.GetOrDefault(connectionName);
                return new ServiceBusAdministrationClient(config.ConnectionString, config.Admin);
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
        if (!_clients.Any())
        {
            Logger.LogDebug($"Disposed connection pool with no connection in the pool.");
            return;
        }

        Logger.LogInformation($"Disposing connection pool ({_clients.Count} connections).");

        foreach (var connection in _clients.Values)
        {
            await connection.Value.DisposeAsync();
        }

        _clients.Clear();

        if (!_adminClients.Any())
        {
            Logger.LogDebug($"Disposed admin connection pool with no admin connection in the pool.");
            return;
        }

        Logger.LogInformation($"Disposing admin connection pool ({_adminClients.Count} admin connections).");
        _adminClients.Clear();
    }
}
