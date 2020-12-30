using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Progression
{
    public class NullUiPageProgressService : IUiPageProgressService, ISingletonDependency
    {
        public event EventHandler<UiPageProgressEventArgs> ProgressChanged;

        public Task Go(int? percentage, Action<UiPageProgressOptions> options = null)
        {
            return Task.CompletedTask;
        }
    }
}
