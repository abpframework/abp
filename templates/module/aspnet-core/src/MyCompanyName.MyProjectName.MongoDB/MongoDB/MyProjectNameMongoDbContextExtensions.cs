using Volo.Abp;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public static class MyProjectNameMongoDbContextExtensions
    {
        public static void ConfigureMyProjectName(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
        }
    }
}
