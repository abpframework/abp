using System;
using Mongo2Go;
using MongoDB.Driver;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoDbFixture : IDisposable
    {
        private static readonly MongoDbRunner MongoDbRunner;
        public static readonly string ConnectionString;

        static MongoDbFixture()
        {
            MongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true, singleNodeReplSetWaitTimeout: 10);
            ConnectionString = MongoDbRunner.ConnectionString;

            //TODO It can be removed, when Mongo2Go solves this issue : https://github.com/Mongo2Go/Mongo2Go/issues/89
            var client = new MongoClient(MongoDbRunner.ConnectionString);
            client.EnsureReplicationSetReady();
        }

        public void Dispose()
        {
            MongoDbRunner?.Dispose();
        }
    }
}
