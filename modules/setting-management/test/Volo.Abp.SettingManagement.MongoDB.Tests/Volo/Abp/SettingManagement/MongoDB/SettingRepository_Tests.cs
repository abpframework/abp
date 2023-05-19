using Xunit;

namespace Volo.Abp.SettingManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class SettingRepository_Tests : SettingRepository_Tests<AbpSettingManagementMongoDbTestModule>
{

}
