using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class NullUiNotificationService : IUiNotificationService, ITransientDependency
    {
        public Task Info(string message)
        {
            return Task.CompletedTask;
        }

        public Task Success(string message)
        {
            return Task.CompletedTask;
        }

        public Task Warn(string message)
        {
            return Task.CompletedTask;
        }
        public Task Error(string message)
        {
            return Task.CompletedTask;
        }
    }
}