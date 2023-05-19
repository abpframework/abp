using System.Threading.Tasks;

namespace Volo.Abp.Application.Services;

public interface IDeleteAppService<in TKey> : IApplicationService
{
    Task DeleteAsync(TKey id);
}
