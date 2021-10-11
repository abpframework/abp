using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Volo.Abp.Identity
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
    [Area("identity")]
    [ControllerName("UserLookup")]
    [Route("api/identity/users/lookup")]
    public class IdentityUserLookupController : AbpControllerBase, IIdentityUserLookupAppService
    {
        protected IIdentityUserLookupAppService LookupAppService { get; }

        public IdentityUserLookupController(IIdentityUserLookupAppService lookupAppService)
        {
            LookupAppService = lookupAppService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<UserData> FindByIdAsync(Guid id)
        {
            return LookupAppService.FindByIdAsync(id);
        }

        [HttpGet]
        [Route("by-username/{userName}")]
        public virtual Task<UserData> FindByUserNameAsync(string userName)
        {
            return LookupAppService.FindByUserNameAsync(userName);
        }

        [HttpGet]
        [Route("search")]
        public Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
        {
            return LookupAppService.SearchAsync(input);
        }

        [HttpGet]
        [Route("count")]
        public Task<long> GetCountAsync(UserLookupCountInputDto input)
        {
            return LookupAppService.GetCountAsync(input);
        }
    }
}
