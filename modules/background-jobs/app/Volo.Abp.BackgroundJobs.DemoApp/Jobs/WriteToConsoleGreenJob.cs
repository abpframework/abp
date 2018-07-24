using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs
{
    public class WriteToConsoleGreenJob : BackgroundJob<WriteToConsoleGreenJobArgs>, ITransientDependency
    {
        public override void Execute(WriteToConsoleGreenJobArgs args)
        {
            if (RandomHelper.GetRandomOf(1, 2) == 2)
            {
                throw new ApplicationException("A sample exception from the WriteToConsoleGreenJob!");
            }

            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"############### WriteToConsoleGreenJob: {args.Value} ###############");
            Console.ForegroundColor = oldColor;
        }
    }
}