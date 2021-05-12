using System.Threading.Tasks;

namespace Volo.Docs.Admin.Notification
{
    public interface IUiNotificationClient
    {
        Task SendNotification(string message);
    }
}