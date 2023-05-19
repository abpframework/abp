using MongoDB.Bson.Serialization;

namespace Volo.Abp.MongoDB;

public interface IHasBsonClassMap
{
    BsonClassMap GetMap();
}
