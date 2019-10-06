using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.BookManagement.MongoDB
{
    public static class BookManagementMongoDbContextExtensions
    {
        public static void ConfigureBookManagement(
            this IMongoModelBuilder builder,
            Action<MongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BookManagementMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);
        }
    }
}