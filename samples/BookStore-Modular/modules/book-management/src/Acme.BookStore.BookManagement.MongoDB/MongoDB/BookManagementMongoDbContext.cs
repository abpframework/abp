using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.BookManagement.MongoDB
{
    [ConnectionStringName("BookManagement")]
    public class BookManagementMongoDbContext : AbpMongoDbContext, IBookManagementMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = BookManagementConsts.DefaultDbTablePrefix;

        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureBookManagement(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}