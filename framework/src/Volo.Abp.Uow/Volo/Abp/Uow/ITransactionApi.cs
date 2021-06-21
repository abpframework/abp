using System;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface ITransactionApi : IDisposable
    {
        Task CommitAsync();
    }
}