using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [Dependency(TryRegister = true)]
    public class HttpClientExternalUserLookupServiceProvider : IExternalUserLookupServiceProvider, ITransientDependency
    {
        private readonly IIdentityUserLookupAppService _userLookupAppService;

        public HttpClientExternalUserLookupServiceProvider(IIdentityUserLookupAppService userLookupAppService)
        {
            _userLookupAppService = userLookupAppService;
        }

        public async Task<IUserData> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _userLookupAppService.FindByIdAsync(id);
        }

        public async Task<IUserData> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await _userLookupAppService.FindByUserNameAsync(userName);
        }
    }
}
