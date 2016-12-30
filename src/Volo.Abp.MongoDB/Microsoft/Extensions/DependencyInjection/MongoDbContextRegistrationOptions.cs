using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public class MongoDbContextRegistrationOptions : CommonDbContextRegistrationOptions, IMongoDbContextRegistrationOptionsBuilder
    {

    }
}