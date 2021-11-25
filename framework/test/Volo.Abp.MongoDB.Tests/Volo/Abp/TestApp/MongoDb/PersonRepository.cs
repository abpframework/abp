using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.MongoDB;

public class PersonRepository : MongoDbRepository<ITestAppMongoDbContext, Person, Guid>, IPersonRepository
{
    public PersonRepository(IMongoDbContextProvider<ITestAppMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public async Task<PersonView> GetViewAsync(string name)
    {
        var person = await (await (await GetCollectionAsync()).FindAsync(x => x.Name == name)).FirstOrDefaultAsync();
        return new PersonView()
        {
            Name = person.Name,
            CreationTime = person.CreationTime,
            Birthday = person.Birthday,
            LastActive = person.LastActive
        };
    }
}
