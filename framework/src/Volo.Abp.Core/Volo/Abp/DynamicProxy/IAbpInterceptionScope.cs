using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.DynamicProxy;

public interface IAbpInterceptionScope : IDisposable
{
    /// <summary>
    /// Also called when the caller stops enumerating early.
    /// </summary>
    Task CompleteAsync(CancellationToken cancellationToken);

    Task FailAsync(Exception exception);
}
