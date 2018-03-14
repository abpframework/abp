using System;
using System.Threading.Tasks;

namespace Volo.Abp.Users
{
    public interface IUserLookupService
    {
        Task<IUserInfo> FindByIdAsync(Guid id);

        Task<IUserInfo> FindByUserNameAsync(string userName);

        //TODO: Searching users...
    }
}
