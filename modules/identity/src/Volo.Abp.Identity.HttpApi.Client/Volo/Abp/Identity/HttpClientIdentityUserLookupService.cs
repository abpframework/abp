using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [Dependency(TryRegister = true)]
    public class HttpClientExternalUserLookupServiceProvider : IExternalUserLookupServiceProvider, ITransientDependency
    {
        private readonly IIdentityUserAppService _userAppService;

        public HttpClientExternalUserLookupServiceProvider(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task<IUserData> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //TODO: Should return null if not found!
            return (await _userAppService.GetAsync(id)).ToUserInfo();
        }

        public async Task<IUserData> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            //TODO: Should return null if not found!
            //TODO: Search by UserName, not by a general filter!
            return (await _userAppService.GetListAsync(new GetIdentityUsersInput { Filter = userName })).Items.FirstOrDefault()?.ToUserInfo();
        }
    }
}
