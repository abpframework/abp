using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyModuleName.MongoDB
{
    [ConnectionStringName("MyModuleName")]
    public interface IMyModuleNameMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
    }
}
