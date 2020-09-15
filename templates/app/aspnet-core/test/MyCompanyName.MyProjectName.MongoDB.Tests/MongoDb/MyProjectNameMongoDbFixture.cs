using System;
using Mongo2Go;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public class MyProjectNameMongoDbFixture : IDisposable
    {
        private static readonly MongoDbRunner MongoDbRunner;
        public static readonly string ConnectionString;

        static MyProjectNameMongoDbFixture()
        {
            MongoDbRunner = MongoDbRunner.Start();
            ConnectionString = MongoDbRunner.ConnectionString;
        }

        public void Dispose()
        {
            MongoDbRunner?.Dispose();
        }
    }
}
