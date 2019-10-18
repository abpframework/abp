using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.BookManagement.MongoDB
{
    public class BookManagementMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public BookManagementMongoModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = BookManagementConsts.DefaultDbTablePrefix)
            : base(tablePrefix)
        {
        }
    }
}