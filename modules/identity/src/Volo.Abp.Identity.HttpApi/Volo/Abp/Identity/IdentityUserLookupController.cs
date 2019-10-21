using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [RemoteService]
    [Area("identity")]
    [ControllerName("UserLookup")]
    [Route("api/identity/users/lookup")]
    public class IdentityUserLookupController : AbpController, IIdentityUserLookupAppService
    {
        protected IIdentityUserLookupAppService LookupAppService { get; }

        public IdentityUserLookupController(IIdentityUserLookupAppService lookupAppService)
        {
            LookupAppService = lookupAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<UserData> FindByIdAsync(Guid id)
        {
            return LookupAppService.FindByIdAsync(id);
        }

        [HttpGet]
        [Route("by-username/{userName}")]
        public Task<UserData> FindByUserNameAsync(string userName)
        {
            return LookupAppService.FindByUserNameAsync(userName);
        }
    }
}
