using System.Threading.Tasks;

namespace Volo.Docs.Admin.Pages.Docs.Admin.SignalR
{
    public interface INotificationClient
    {
        Task ReceiveNotificationMessage(string message);
    }
}