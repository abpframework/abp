using System.Threading;

namespace Volo.Abp.Threading;

public class NullCancellationTokenProvider : CancellationTokenProviderBase
{
    public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

    public override CancellationToken Token => OverrideValue?.CancellationToken ?? CancellationToken.None;

    private NullCancellationTokenProvider()
        : base(new AmbientDataContextAmbientScopeProvider<CancellationTokenOverride>(new AsyncLocalAmbientDataContext()))
    {
    }
}
