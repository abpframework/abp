using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Volo.Abp.Identity.Integration;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("UserIntegration")]
[Route("integration-api/identity/users")]
public class IdentityUserIntegrationController : AbpControllerBase, IIdentityUserIntegrationService
{
    protected IIdentityUserIntegrationService UserIntegrationService { get; }

    public IdentityUserIntegrationController(IIdentityUserIntegrationService userIntegrationService)
    {
        UserIntegrationService = userIntegrationService;
    }
    
    [HttpGet]
    [Route("{id}/role-names")]
    public virtual Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return UserIntegrationService.GetRoleNamesAsync(id);
    }

    [HttpGet]
    [Route("{id}")]
    public Task<UserData> FindByIdAsync(Guid id)
    {
        return UserIntegrationService.FindByIdAsync(id);
    }

    [HttpGet]
    [Route("by-username/{userName}")]
    public Task<UserData> FindByUserNameAsync(string userName)
    {
        return UserIntegrationService.FindByUserNameAsync(userName);
    }

    [HttpGet]
    [Route("search")]
    public Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        return UserIntegrationService.SearchAsync(input);
    }

    [HttpGet]
    [Route("count")]
    public Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return UserIntegrationService.GetCountAsync(input);
    }
}