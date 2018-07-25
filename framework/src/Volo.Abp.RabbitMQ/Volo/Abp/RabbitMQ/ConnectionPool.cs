using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.RabbitMQ
{
    public class ConnectionPool : IConnectionPool, ISingletonDependency
    {
        protected AbpRabbitMqOptions Options { get; }

        protected ConcurrentDictionary<string, IConnection> Connections { get; }

        public ConnectionPool(IOptions<AbpRabbitMqOptions> options)
        {
            Options = options.Value;
            Connections = new ConcurrentDictionary<string, IConnection>();
        }

        public virtual IConnection Get(string connectionName = null)
        {
            return Connections.GetOrAdd(connectionName, () =>
            {
                var connectionFactory = Options.Connections.GetOrDefault(connectionName)
                                        ?? Options.Connections.Default;

                return connectionFactory.CreateConnection();
            });
        }
    }
}