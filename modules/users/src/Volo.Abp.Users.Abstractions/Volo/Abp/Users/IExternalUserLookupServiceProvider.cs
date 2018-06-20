using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Users
{
    public interface IExternalUserLookupServiceProvider //TODO: Consider to inherit from IUserLookupService
    {
        Task<IUserData> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IUserData> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    }
}