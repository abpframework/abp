using System;
using Mongo2Go;
using MongoDB.Driver;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public class MyProjectNameMongoDbFixture : IDisposable
    {
        private static readonly MongoDbRunner MongoDbRunner;
        public static readonly string ConnectionString;

        static MyProjectNameMongoDbFixture()
        {
            MongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true, singleNodeReplSetWaitTimeout: 10);
            ConnectionString = MongoDbRunner.ConnectionString;

            var client = new MongoClient(MongoDbRunner.ConnectionString);
            client.EnsureReplicationSetReady();
        }

        public void Dispose()
        {
            MongoDbRunner?.Dispose();
        }
    }
}
