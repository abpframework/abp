using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Volo.Abp.BackgroundJobs
{
    public abstract class AsyncBackgroundJob<TArgs> : IAsyncBackgroundJob<TArgs>
    {
        //TODO: Add UOW, Localization and other useful properties..?

        public ILogger<AsyncBackgroundJob<TArgs>> Logger { get; set; }

        protected AsyncBackgroundJob()
        {
            Logger = NullLogger<AsyncBackgroundJob<TArgs>>.Instance;
        }

        public abstract Task ExecuteAsync(TArgs args);
    }
}