using System;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobNameAttribute : Attribute, IBackgroundJobNameProvider
    {
        public string Name { get; }

        public BackgroundJobNameAttribute([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public static string GetName<TJobArgs>()
        {
            return GetName(typeof(TJobArgs));
        }

        public static string GetName([NotNull] Type jobArgsType)
        {
            Check.NotNull(jobArgsType, nameof(jobArgsType));

            return jobArgsType
                       .GetCustomAttributes(true)
                       .OfType<IBackgroundJobNameProvider>()
                       .FirstOrDefault()
                       ?.Name
                   ?? jobArgsType.FullName;
        }
    }
}
