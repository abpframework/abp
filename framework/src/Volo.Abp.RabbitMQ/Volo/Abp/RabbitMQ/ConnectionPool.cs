using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.RabbitMQ;

public class ConnectionPool : IConnectionPool, ISingletonDependency
{
    protected AbpRabbitMqOptions Options { get; }

    protected ConcurrentDictionary<string, IConnection> Connections { get; }

    protected SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

    private bool _isDisposed;

    public ConnectionPool(IOptions<AbpRabbitMqOptions> options)
    {
        Options = options.Value;
        Connections = new ConcurrentDictionary<string, IConnection>();
    }

    public virtual async Task<IConnection> GetAsync(string? connectionName = null)
    {
        using (await Semaphore.LockAsync())
        {
            connectionName ??= RabbitMqConnections.DefaultConnectionName;

            if (Connections.TryGetValue(connectionName, out var existingConnection) && existingConnection.IsOpen)
            {
                return existingConnection;
            }

            if(existingConnection != null)
            {
                await existingConnection.DisposeAsync();
            }

            var connectionFactory = Options.Connections.GetOrDefault(connectionName);
            var connection = await GetConnectionAsync(connectionName, connectionFactory);
            Connections[connectionName] = connection;
            return connection;
        }
    }

    protected virtual async Task<IConnection> GetConnectionAsync(string connectionName, ConnectionFactory connectionFactory)
    {
        var hostnames = connectionFactory.HostName.TrimEnd(';').Split(';');
        // Handle Rabbit MQ Cluster.
        return hostnames.Length == 1
            ? await connectionFactory.CreateConnectionAsync()
            : await connectionFactory.CreateConnectionAsync(hostnames);
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        foreach (var connection in Connections.Values)
        {
            try
            {
                await connection.DisposeAsync();
            }
            catch
            {
                // ignored
            }
        }

        Connections.Clear();
    }
}
