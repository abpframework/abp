using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow;

public interface ITransactionApi : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
