using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Volo.Abp.BackgroundJobs;

public abstract class BackgroundJob<TArgs> : IBackgroundJob<TArgs>
{
    //TODO: Add UOW, Localization and other useful properties..?

    public ILogger<BackgroundJob<TArgs>> Logger { get; set; }

    protected BackgroundJob()
    {
        Logger = NullLogger<BackgroundJob<TArgs>>.Instance;
    }

    public abstract void Execute(TArgs args);
}
