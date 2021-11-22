using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Users;

public interface IUserLookupService<TUser>
    where TUser : class, IUser
{
    Task<TUser> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<TUser> FindByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default
    );

    Task<List<IUserData>> SearchAsync(
        string sorting = null,
        string filter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string filter = null,
        CancellationToken cancellationToken = default);
}
