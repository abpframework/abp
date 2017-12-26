using System.Threading;

namespace Volo.Abp.Threading
{
    public static class CancellationTokenProviderExtensions
    {
        public static CancellationToken FallbackToProvider(this ICancellationTokenProvider provider, CancellationToken prefferedValue)
        {
            return prefferedValue == default ? provider.Token : prefferedValue;
        }
    }
}