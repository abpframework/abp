using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public interface IUiNotificationService
    {
        Task Info(string message);
    }
}
