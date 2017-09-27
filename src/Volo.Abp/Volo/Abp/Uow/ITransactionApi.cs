using System;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface ITransactionApi : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }
}