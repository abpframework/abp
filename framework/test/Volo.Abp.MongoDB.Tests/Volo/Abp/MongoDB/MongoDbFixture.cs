using System;
using Mongo2Go;

namespace Volo.Abp.MongoDB
{
    public class MongoDbFixture : IDisposable
    {
        public static MongoDbRunner MongoDbRunner;

        public MongoDbFixture()
        {
            MongoDbRunner = MongoDbRunner.Start();
        }

        public static string GetConnectionString()
        {
            return MongoDbRunner.ConnectionString;
        }
        
        public void Dispose()
        {
            MongoDbRunner?.Dispose();
        }
    }
}