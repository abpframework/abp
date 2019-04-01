using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public interface IIdentityUserLookupAppService : IApplicationService
    {
        Task<UserData> FindByIdAsync(Guid id);

        Task<UserData> FindByUserNameAsync(string userName);
    }
}
