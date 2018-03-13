using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Users
{
    public class NullUserLookupService : IUserLookupService, ISingletonDependency
    {
        private static readonly Task<IUserInfo> NullUserResult = Task.FromResult((IUserInfo) null);

        public Task<IUserInfo> FindUserByIdAsync(Guid id)
        {
            return NullUserResult;
        }

        public Task<IUserInfo> FindUserByUserNameAsync(string userName)
        {
            return NullUserResult;
        }
    }
}