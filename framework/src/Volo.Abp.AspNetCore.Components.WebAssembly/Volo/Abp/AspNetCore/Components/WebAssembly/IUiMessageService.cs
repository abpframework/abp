using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public interface IUiMessageService
    {
        Task InfoAsync(string message, string title = null);
        
        Task SuccessAsync(string message, string title = null);
        
        Task WarnAsync(string message, string title = null);
        
        Task ErrorAsync(string message, string title = null);
        
        Task<bool> ConfirmAsync(string message, string title = null);
    }
}
