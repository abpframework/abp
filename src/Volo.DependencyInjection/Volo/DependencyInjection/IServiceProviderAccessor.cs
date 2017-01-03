using System;
using JetBrains.Annotations;

namespace Volo.DependencyInjection
{
    public interface IServiceProviderAccessor
    {
        [NotNull]
        IServiceProvider ServiceProvider { get; }
    }
}