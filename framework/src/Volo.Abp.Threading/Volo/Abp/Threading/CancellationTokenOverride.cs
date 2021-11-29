using System.Threading;

namespace Volo.Abp.Threading
{
    public class CancellationTokenOverride
    {
        public CancellationToken CancellationToken { get; }

        public CancellationTokenOverride(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }
    }
}
