using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App
{
    public interface IHelloController : IRemoteService
    {
        Task<string> PostAsync();
    }
}
