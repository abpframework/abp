using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public interface IUiMessageService
    {
        Task<bool> ConfirmAsync(string message, string title = null);
    }
}
