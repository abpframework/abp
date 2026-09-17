using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

public class AbpTickerQBackgroundWorkersProvider : ISingletonDependency
{
    public Dictionary<string, AbpTickerQCronBackgroundWorker> BackgroundWorkers { get;}

    public AbpTickerQBackgroundWorkersProvider()
    {
        BackgroundWorkers = new Dictionary<string, AbpTickerQCronBackgroundWorker>();
    }
}
