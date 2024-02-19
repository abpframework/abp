using MongoDB.Driver;

namespace Volo.Abp.MongoDB;

public interface IHasCreateCollectionOptions
{
    CreateCollectionOptions GetCreateCollectionOptions();
}