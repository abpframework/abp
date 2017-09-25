using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface ITransactionApi
    {
        void Commit();

        Task CommitAsync();

        void Dispose();
    }
}