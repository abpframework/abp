using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs
{
    public class MyJob : BackgroundJob<MyJobArgs>, ISingletonDependency
    {
        public List<string> ExecutedValues { get; } = new List<string>();

        public override void Execute(MyJobArgs args)
        {
            ExecutedValues.Add(args.Value);
        }
    }
}