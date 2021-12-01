namespace Volo.Abp.MongoDB;

public interface IMongoModelSource
{
    MongoDbContextModel GetModel(AbpMongoDbContext dbContext);
}
