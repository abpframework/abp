using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs
{
    public class WriteToConsoleYellowJob : BackgroundJob<WriteToConsoleYellowJobArgs>, ITransientDependency
    {
        public override void Execute(WriteToConsoleYellowJobArgs args)
        {
            if (RandomHelper.GetRandomOf(1, 2) == 2)
            {
                throw new ApplicationException("A sample exception from the WriteToConsoleYellowJob!");
            }

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"############### WriteToConsoleYellowJob: {args.Value} ###############");
            Console.ForegroundColor = oldColor;
        }
    }
}