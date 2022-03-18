using System;
using System.Threading;

namespace Volo.Abp.Threading;

public interface ICancellationTokenProvider
{
    CancellationToken Token { get; }

    IDisposable Use(CancellationToken cancellationToken);
}
