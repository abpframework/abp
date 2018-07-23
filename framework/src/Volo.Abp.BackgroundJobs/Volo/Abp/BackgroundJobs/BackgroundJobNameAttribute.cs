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

        public static string GetNameOrNull<TJobArgs>()
        {
            return GetNameOrNull(typeof(TJobArgs));
        }

        public static string GetNameOrNull(Type jobArgsType)
        {
            return jobArgsType
                .GetCustomAttributes(true)
                .OfType<IBackgroundJobNameProvider>()
                .FirstOrDefault()
                ?.Name;
        }
    }
}
