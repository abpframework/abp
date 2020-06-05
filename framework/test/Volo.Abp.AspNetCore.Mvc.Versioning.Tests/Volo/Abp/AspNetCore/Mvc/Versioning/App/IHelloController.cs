using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.Versioning.App
{
    public interface IHelloController : IRemoteService
    {
        Task<string> GetAsync();

        Task<string> PostAsyncV1();

        Task<string> PostAsyncV2();
    }
}
