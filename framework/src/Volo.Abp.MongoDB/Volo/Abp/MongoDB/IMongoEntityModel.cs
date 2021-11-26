using System;

namespace Volo.Abp.MongoDB;

public interface IMongoEntityModel
{
    Type EntityType { get; }

    string CollectionName { get; }
}
