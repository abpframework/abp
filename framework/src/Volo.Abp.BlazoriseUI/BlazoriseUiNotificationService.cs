using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI
{
    [Dependency(ReplaceServices = true)]
    public class BlazoriseUiNotificationService : IUiNotificationService, ITransientDependency
    {
        public ILogger<BlazoriseUiNotificationService> Logger { get; set; }

        public BlazoriseUiNotificationService()
        {
            Logger = NullLogger<BlazoriseUiNotificationService>.Instance;
        }

        public Task Info(string message)
        {
            Logger.LogInformation(message);
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
