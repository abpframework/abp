using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.MongoDB.Uow
{
    [Collection("MongoDB Collection")]
    public class Uow_Completed_Tests : Uow_Completed_Tests<AbpMongoDbTestModule>
    {
    }
}