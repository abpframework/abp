using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    [ConnectionStringName("MyProjectName")]
    public class MyProjectNameMongoDbContext : AbpMongoDbContext, IMyProjectNameMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = MyProjectNameConsts.DefaultDbTablePrefix;

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureMyProjectName(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}