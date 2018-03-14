using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    public class HttpClientIdentityUserLookupService : IUserLookupService, ITransientDependency
    {
        private readonly IIdentityUserAppService _userAppService;

        public HttpClientIdentityUserLookupService(IIdentityUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task<IUserInfo> FindByIdAsync(Guid id)
        {
            //TODO: Should return null if not found!
            return (await _userAppService.GetAsync(id)).ToUserInfo();
        }

        public async Task<IUserInfo> FindByUserNameAsync(string userName)
        {
            //TODO: Should return null if not found!
            //TODO: Search by UserName, not by a general filter!
            return (await _userAppService.GetListAsync(new GetIdentityUsersInput { Filter = userName })).Items.FirstOrDefault()?.ToUserInfo();
        }
    }
}
