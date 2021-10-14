using System.Threading.Tasks;
using Medallion.Threading;

namespace Volo.Abp.DistributedLocking
{
    public class MedallionAbpDistributedLockHandle : IAbpDistributedLockHandle
    {
        public IDistributedSynchronizationHandle Handle { get; }

        public MedallionAbpDistributedLockHandle(IDistributedSynchronizationHandle handle)
        {
            Handle = handle;
        }

        public ValueTask DisposeAsync()
        {
            return Handle.DisposeAsync();
        }
    }
}