using System.Threading.Tasks;

namespace Volo.Abp.Uow
{
    public interface IDatabaseApi
    {
        Task SaveChangesAsync();

        Task CommitAsync();
    }
}