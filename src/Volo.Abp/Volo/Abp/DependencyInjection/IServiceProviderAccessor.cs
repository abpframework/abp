using System;
using JetBrains.Annotations;

namespace Volo.Abp.DependencyInjection
{
    public interface IServiceProviderAccessor //TODO: Move to Volo.DependencyInjection package?
    {
        [NotNull]
        IServiceProvider ServiceProvider { get; }
    }
}