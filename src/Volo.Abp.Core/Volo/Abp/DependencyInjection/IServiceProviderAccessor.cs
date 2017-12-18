using System;
using JetBrains.Annotations;

namespace Volo.Abp.DependencyInjection
{
    public interface IServiceProviderAccessor
    {
        [NotNull]
        IServiceProvider ServiceProvider { get; }
    }
}