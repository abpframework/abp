using System;
using System.Threading.Tasks;

namespace Volo.Abp.Users
{
    public interface IUserLookupService
    {
        Task<IUserInfo> FindUserByIdAsync(Guid id);

        Task<IUserInfo> FindUserByUserNameAsync(string userName);

        //TODO: Searching users...
    }
}
