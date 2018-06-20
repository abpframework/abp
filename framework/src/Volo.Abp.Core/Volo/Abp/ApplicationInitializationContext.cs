using System;
using JetBrains.Annotations;

namespace Volo.Abp
{
    public class ApplicationInitializationContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ApplicationInitializationContext([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            ServiceProvider = serviceProvider;
        }
    }
}