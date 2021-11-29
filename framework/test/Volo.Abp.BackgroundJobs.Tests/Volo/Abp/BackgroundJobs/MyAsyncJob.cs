using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs
{
    public class MyAsyncJob : AsyncBackgroundJob<MyAsyncJobArgs>, ISingletonDependency
    {
        public List<string> ExecutedValues { get; } = new List<string>();

        public override Task ExecuteAsync(MyAsyncJobArgs args)
        {
            ExecutedValues.Add(args.Value);

            return Task.CompletedTask;
        }
    }
}
