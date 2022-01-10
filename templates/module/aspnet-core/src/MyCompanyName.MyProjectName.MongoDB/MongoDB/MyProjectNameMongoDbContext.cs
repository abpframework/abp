using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB;

[ConnectionStringName(MyProjectNameDbProperties.ConnectionStringName)]
public class MyProjectNameMongoDbContext : AbpMongoDbContext, IMyProjectNameMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureMyProjectName();
    }
}
