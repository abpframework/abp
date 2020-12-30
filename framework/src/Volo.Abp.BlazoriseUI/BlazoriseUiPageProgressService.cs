using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Progression;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI
{
    [Dependency(ReplaceServices = true)]
    public class BlazoriseUiPageProgressService : IUiPageProgressService,
        // WARNING: this is made Singleton because there was an issue with ABP internal DI.
        // TODO: make it Scoped once the issue is solved.
        ISingletonDependency
    {
        /// <summary>
        /// An event raised after the notification is received.
        /// </summary>
        public event EventHandler<UiPageProgressEventArgs> ProgressChanged;

        public Task Go(int? percentage, Action<UiPageProgressOptions> options = null)
        {
            var uiPageProgressOptions = CreateDefaultOptions();
            options?.Invoke(uiPageProgressOptions);

            ProgressChanged?.Invoke(this, new UiPageProgressEventArgs(percentage, uiPageProgressOptions));

            return Task.CompletedTask;
        }

        protected virtual UiPageProgressOptions CreateDefaultOptions()
        {
            return new UiPageProgressOptions();
        }
    }
}
