using System;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public interface IDatabaseApiContainer : IServiceProviderAccessor
    {
        [CanBeNull]
        IDatabaseApi FindDatabaseApi([NotNull] string id);

        [NotNull]
        IDatabaseApi GetOrAddDatabaseApi([NotNull] string id, [NotNull] Func<IDatabaseApi> factory);
    }
}