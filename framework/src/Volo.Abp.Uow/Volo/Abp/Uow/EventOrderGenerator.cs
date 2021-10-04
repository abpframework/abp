using System.Threading;

namespace Volo.Abp.Uow
{
    public static class EventOrderGenerator
    {
        private static long _lastOrder;

        public static long GetNext()
        {
            return Interlocked.Increment(ref _lastOrder);
        }
    }
}