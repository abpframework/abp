using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs
{
    public class WriteToConsoleJob : BackgroundJob<WriteToConsoleJobArgs>, ITransientDependency
    {
        public override void Execute(WriteToConsoleJobArgs args)
        {
            Console.WriteLine($"WriteToConsoleJob: {args.Value}");
        }
    }
}