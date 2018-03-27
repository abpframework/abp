namespace Volo.Abp.MongoDB
{
    public interface IMongoDbContextProvider<out TMongoDbContext>
        where TMongoDbContext : IAbpMongoDbContext
    {
        TMongoDbContext GetDbContext();
    }
}