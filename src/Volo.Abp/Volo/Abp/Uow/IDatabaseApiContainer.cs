using System;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public interface IDatabaseApiContainer : IServiceProviderAccessor
    {
        [CanBeNull]
        IDatabaseApi FindDatabaseApi([NotNull] string key);

        [NotNull]
        IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory);
    }
}