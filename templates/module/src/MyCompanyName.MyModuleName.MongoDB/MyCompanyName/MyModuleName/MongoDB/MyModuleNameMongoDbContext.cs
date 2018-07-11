using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyModuleName.MongoDB
{
    [ConnectionStringName("MyModuleName")]
    public class MyModuleNameMongoDbContext : AbpMongoDbContext, IMyModuleNameMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = MyModuleNameConsts.DefaultDbTablePrefix;

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureMyModuleName(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}