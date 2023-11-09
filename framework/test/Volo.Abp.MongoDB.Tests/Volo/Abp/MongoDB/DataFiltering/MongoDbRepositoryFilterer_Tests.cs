using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.MongoDB.DataFiltering;

[Collection(MongoTestCollection.Name)]
public class MongoDbAsyncQueryableProvider_Tests : MongoDbTestBase
{
    private readonly IRepository<City, Guid> _personRepository;

    protected override void AfterAddApplication(IServiceCollection services)
    {
        services.AddTransient<IMongoDbRepositoryFilterer<City, Guid>, TestFilterQueryable>();
        base.AfterAddApplication(services);
    }

    public MongoDbAsyncQueryableProvider_Tests()
    {
        _personRepository = GetRequiredService<IRepository<City, Guid>>();
    }

    [Fact]
    public async Task Test()
    {
        var cities = await _personRepository.GetListAsync();
        cities.Count.ShouldBe(1);
        cities[0].Name.ShouldBe("Istanbul");
    }
}

public class TestFilterQueryable : MongoDbRepositoryFilterer<City, Guid>
{
    public TestFilterQueryable(
        IDataFilter dataFilter,
        ICurrentTenant currentTenant) :
        base(dataFilter, currentTenant)
    {
    }

    public override TQueryable FilterQueryable<TQueryable>(TQueryable query)
    {
        return (TQueryable)query.Where(p => p.Name == "Istanbul");
    }
}
