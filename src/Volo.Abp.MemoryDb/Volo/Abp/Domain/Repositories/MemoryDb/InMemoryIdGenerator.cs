using System;
using System.Threading;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    internal class InMemoryIdGenerator
    {
        private int _lastInt;
        private long _lastLong;

        public TPrimaryKey GenerateNext<TPrimaryKey>()
        {
            if (typeof(TPrimaryKey) == typeof(Guid))
            {
                return (TPrimaryKey)(object)Guid.NewGuid();
            }

            if (typeof(TPrimaryKey) == typeof(int))
            {
                return (TPrimaryKey)(object)Interlocked.Increment(ref _lastInt);
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return (TPrimaryKey)(object)Interlocked.Increment(ref _lastLong);
            }

            throw new AbpException("Not supported PrimaryKey type: " + typeof(TPrimaryKey).FullName);
        }
    }
}