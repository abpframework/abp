using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App.v1
{
    public interface ITodoAppService : IApplicationService
    {
        Task<string> GetAsync(int id);
    }
}