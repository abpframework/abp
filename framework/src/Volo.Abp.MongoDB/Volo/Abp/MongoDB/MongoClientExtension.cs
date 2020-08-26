using System;
using System.Threading;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB
{
    /// <summary>
    /// TODO It can be removed, when Mongo2Go solves this issue : https://github.com/Mongo2Go/Mongo2Go/issues/89
    /// </summary>
    public static class MongoClientExtension
    {
        private static readonly TimeSpan InitialDelay = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan MaxDelay = TimeSpan.FromSeconds(5000);


        public static void EnsureReplicationSetReady(this IMongoClient mongoClient)
        {
            var delay = InitialDelay;
            var database = mongoClient.GetDatabase("__dummy-db");
            try
            {
                while (true)
                {
                    try
                    {
                        _ = database.GetCollection<DummyEntry>("__dummy");
                        database.DropCollection("__dummy");

                        var session = mongoClient.StartSession();

                        try
                        {
                            session.StartTransaction();
                            session.AbortTransaction();
                        }
                        finally
                        {
                            session.Dispose();
                        }

                        break;
                    }
                    catch (NotSupportedException)
                    {
                    }

                    Thread.Sleep(delay);
                    delay = Min(Double(delay), MaxDelay);
                }
            }
            finally
            {
                mongoClient.DropDatabase("__dummy-db");
            }
        }

        private static TimeSpan Min(TimeSpan left, TimeSpan right)
        {
            return new TimeSpan(Math.Min(left.Ticks, right.Ticks));
        }

        private static TimeSpan Double(TimeSpan timeSpan)
        {
            long ticks;
            try
            {
                ticks = checked(timeSpan.Ticks * 2);
            }
            catch (OverflowException)
            {
                if (timeSpan.Ticks >= 0)
                {
                    return TimeSpan.MaxValue;
                }

                return TimeSpan.MinValue;
            }

            return new TimeSpan(ticks);
        }

        private sealed class DummyEntry
        {
            public int Id { get; set; }
        }
    }
}
