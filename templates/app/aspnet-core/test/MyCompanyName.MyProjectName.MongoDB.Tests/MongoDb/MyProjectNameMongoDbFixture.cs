using System;
using EphemeralMongo;

namespace MyCompanyName.MyProjectName.MongoDB;

public class MyProjectNameMongoDbFixture : IDisposable
{
    public readonly static IMongoRunner MongoDbRunner;

    static MyProjectNameMongoDbFixture()
    {
        MongoDbRunner = MongoRunner.Run(new MongoRunnerOptions
        {
            UseSingleNodeReplicaSet = true
        });
    }

    public static string GetRandomConnectionString()
    {
        return GetConnectionString("Db_" + Guid.NewGuid().ToString("N"));
    }

    public static string GetConnectionString(string databaseName)
    {
        var stringArray = MongoDbRunner.ConnectionString.Split('?');
        var connectionString = stringArray[0].EnsureEndsWith('/') + databaseName + "/?" + stringArray[1];
        return connectionString;
    }

    public void Dispose()
    {
        MongoDbRunner?.Dispose();
    }
}
