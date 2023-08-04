﻿using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    public class WriteToConsoleYellowJob : BackgroundJob<WriteToConsoleYellowJobArgs>, ITransientDependency
    {
        public override void Execute(WriteToConsoleYellowJobArgs args)
        {
            if (RandomHelper.GetRandom(0, 100) < 70)
            {
                //throw new ApplicationException("A sample exception from the WriteToConsoleYellowJob!");
            }

            lock (Console.Out)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine();
                Console.WriteLine($"############### WriteToConsoleYellowJob: {args.Value} - {args.Time:HH:mm:ss}  ###############");
                Console.WriteLine();

                Console.ForegroundColor = oldColor;
            }
        }
    }
}