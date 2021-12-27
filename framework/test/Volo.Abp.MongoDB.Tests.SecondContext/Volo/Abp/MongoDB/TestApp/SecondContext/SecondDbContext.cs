using MongoDB.Driver;

namespace Volo.Abp.MongoDB.TestApp.SecondContext;

public class SecondDbContext : AbpMongoDbContext
{
    public IMongoCollection<BookInSecondDbContext> Books => Collection<BookInSecondDbContext>();

    public IMongoCollection<PhoneInSecondDbContext> Phones => Collection<PhoneInSecondDbContext>();
}
