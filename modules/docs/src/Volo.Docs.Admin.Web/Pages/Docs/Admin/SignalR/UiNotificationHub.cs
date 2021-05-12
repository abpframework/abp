using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs.Admin.Pages.Docs.Admin.SignalR
{
    public class UiNotificationHub : Hub<INotificationClient>, ITransientDependency
    {
        public Task SendNotification(string message)
        {
            return Clients
                .Client(Context.ConnectionId)
                .ReceiveNotificationMessage(message);
        }
    }
}