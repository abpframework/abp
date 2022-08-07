using System;
using Mongo2Go;

namespace Volo.Abp.MongoDB;

public class MongoDbFixture : IDisposable
{
    private readonly static MongoDbRunner MongoDbRunner;
    public readonly static string ConnectionString;

    static MongoDbFixture()
    {
        MongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true, singleNodeReplSetWaitTimeout: 20);
        ConnectionString = MongoDbRunner.ConnectionString;
    }

    public void Dispose()
    {
        MongoDbRunner?.Dispose();
    }
}
