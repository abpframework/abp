using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Users
{
    public interface IExternalUserLookupServiceProvider
    {
        Task<IUserData> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IUserData> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);

        Task<List<IUserData>> SearchAsync(
            string sorting,
            string filter,
            int maxResultCount,
            CancellationToken cancellationToken = default);
    }
}