using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    [ConnectionStringName("MyProjectName")]
    public interface IMyProjectNameMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
