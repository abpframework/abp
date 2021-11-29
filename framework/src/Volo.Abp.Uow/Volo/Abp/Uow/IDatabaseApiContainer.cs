using System;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public interface IDatabaseApiContainer : IServiceProviderAccessor
    {
        [CanBeNull]
        IDatabaseApi FindDatabaseApi([NotNull] string key);

        void AddDatabaseApi([NotNull] string key, [NotNull] IDatabaseApi api);

        [NotNull]
        IDatabaseApi GetOrAddDatabaseApi([NotNull] string key, [NotNull] Func<IDatabaseApi> factory);
    }
}