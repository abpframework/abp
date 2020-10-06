using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public interface IUiNotificationService
    {
        Task Info(string message);
        Task Success(string message);
        Task Warn(string message);
        Task Error(string message);
    }
}
