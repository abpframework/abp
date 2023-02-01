using System;
using System.Threading;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    public class LongRunningJob : BackgroundJob<LongRunningJobArgs>, ITransientDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public LongRunningJob(ICancellationTokenProvider cancellationTokenProvider)
        {
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public override void Execute(LongRunningJobArgs args)
        {
            lock (Console.Out)
            {
                var oldColor = Console.ForegroundColor;
                try
                {
                    Console.WriteLine($"Long running {args.Value} start: {DateTime.Now}");

                    for (var i = 1; i <= 10; i++)
                    {
                        _cancellationTokenProvider.Token.ThrowIfCancellationRequested();

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