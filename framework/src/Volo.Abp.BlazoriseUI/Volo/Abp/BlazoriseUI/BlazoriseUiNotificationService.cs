using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI
{
    [Dependency(ReplaceServices = true)]
    public class BlazoriseUiNotificationService : IUiNotificationService, ITransientDependency
    {
        public Task Info(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}
