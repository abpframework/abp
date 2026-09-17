using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB.Clients;

public class AbpMongoClientFactory : IAbpMongoClientFactory, ISingletonDependency
{
    protected ConcurrentDictionary<string, MongoClient> ClientCache { get; }
    protected AbpMongoDbContextOptions Options { get; }

    public AbpMongoClientFactory(IOptions<AbpMongoDbContextOptions> options)
    {
        Options = options.Value;
        ClientCache = new ConcurrentDictionary<string, MongoClient>();
    }

    public virtual Task<MongoClient> GetAsync(MongoUrl mongoUrl)
    {
        Check.NotNull(mongoUrl, nameof(mongoUrl));

        return Task.FromResult(
            ClientCache.GetOrAdd(mongoUrl.ToString(), _ =>
            {
                var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
                Options.MongoClientSettingsConfigurer?.Invoke(mongoClientSettings);
                return new MongoClient(mongoClientSettings);
            }));
    }

    public virtual MongoClient Get(MongoUrl mongoUrl)
    {
        Check.NotNull(mongoUrl, nameof(mongoUrl));

        return ClientCache.GetOrAdd(mongoUrl.ToString(), _ =>
        {
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);
            Options.MongoClientSettingsConfigurer?.Invoke(mongoClientSettings);
            return new MongoClient(mongoClientSettings);
        });
    }
}
