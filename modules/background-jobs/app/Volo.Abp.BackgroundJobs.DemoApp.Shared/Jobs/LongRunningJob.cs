using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    public class LongRunningJob : BackgroundJob<LongRunningJobArgs>, ITransientDependency
    {
        public override void Execute(LongRunningJobArgs args, CancellationToken cancellationToken = default)
        {
            lock (Console.Out)
            {
                var oldColor = Console.ForegroundColor;
                try
                {
                    Console.WriteLine($"Long running {args.Value} start: {DateTime.Now}");

                    for (var i = 1; i <= 10; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        Thread.Sleep(1000);
                        Console.WriteLine($"{args.Value} step-{i} done: {DateTime.Now}");
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Long running {args.Value} completed: {DateTime.Now}");
                }
                catch (OperationCanceledException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Long running {args.Value} cancelled!!!");
                }
                finally
                {
                    Console.ForegroundColor = oldColor;
                }
            }
        }
    }
}