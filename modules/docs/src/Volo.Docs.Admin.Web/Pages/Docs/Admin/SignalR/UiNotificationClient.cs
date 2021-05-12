using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;
using Volo.Docs.Admin.Notification;

namespace Volo.Docs.Admin.Pages.Docs.Admin.SignalR
{
    public class UiNotificationClient : IUiNotificationClient, ITransientDependency
    {
        private readonly IHubContext<UiNotificationHub, INotificationClient> _notificationHub;

        public UiNotificationClient(IHubContext<UiNotificationHub, INotificationClient> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public async Task SendNotification(string message)
        {
            await _notificationHub
                .Clients
                .All
                .ReceiveNotificationMessage(message);
        }
    }
}