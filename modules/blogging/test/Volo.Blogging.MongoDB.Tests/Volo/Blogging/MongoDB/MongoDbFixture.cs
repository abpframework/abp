using System;
using Mongo2Go;

namespace Volo.Blogging.MongoDB
{
    public class MongoDbFixture : IDisposable
    {
        private static readonly MongoDbRunner MongoDbRunner = MongoDbRunner.Start();
        public static readonly string ConnectionString = MongoDbRunner.ConnectionString;

        public void Dispose()
        {
            MongoDbRunner?.Dispose();
        }
    }
}