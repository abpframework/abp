using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MongoDB.ConnectionStrings;

[Dependency(ReplaceServices = true)]
public class MongoDBConnectionStringChecker : IConnectionStringChecker, ITransientDependency
{
    public virtual Task<AbpConnectionStringCheckResult> CheckAsync(string connectionString)
    {
        try
        {
            var mongoUrl = MongoUrl.Create(connectionString);
            var client = new MongoClient(mongoUrl);
            client.GetDatabase(mongoUrl.DatabaseName);
            return Task.FromResult(new AbpConnectionStringCheckResult()
            {
                Connected = true,
                DatabaseExists = true
            });
        }
        catch (Exception)
        {
            return Task.FromResult(new AbpConnectionStringCheckResult());
        }
    }
}
