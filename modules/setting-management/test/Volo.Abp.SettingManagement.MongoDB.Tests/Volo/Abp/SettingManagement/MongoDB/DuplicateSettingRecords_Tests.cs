using Xunit;

namespace Volo.Abp.SettingManagement.MongoDB;

[Collection(MongoTestCollection.Name)]
public class DuplicateSettingRecords_Tests : DuplicateSettingRecords_Tests<AbpSettingManagementMongoDbTestModule>
{

}
