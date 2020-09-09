using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace Volo.Abp.EntityFrameworkCore
{
    public static class DatabaseFacadeExtensions
    {
        public static bool IsRelational(this DatabaseFacade database)
        {
#pragma warning disable EF1001 // Internal EF Core API usage.
            return ((IDatabaseFacadeDependenciesAccessor)database).Dependencies is IRelationalDatabaseFacadeDependencies;
#pragma warning restore EF1001 // Internal EF Core API usage.
        }
    }
}
