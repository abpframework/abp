using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface ISupportsSavingChanges
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}