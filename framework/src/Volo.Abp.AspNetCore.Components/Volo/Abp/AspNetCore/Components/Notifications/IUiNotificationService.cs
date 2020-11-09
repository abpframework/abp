using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Notifications
{
    public interface IUiNotificationService
    {
        Task Info(string message, string title = null, Action<UiNotificationOptions> options = null);
        
        Task Success(string message, string title = null, Action<UiNotificationOptions> options = null);
        
        Task Warn(string message, string title = null, Action<UiNotificationOptions> options = null);
        
        Task Error(string message, string title = null, Action<UiNotificationOptions> options = null);
    }
}
