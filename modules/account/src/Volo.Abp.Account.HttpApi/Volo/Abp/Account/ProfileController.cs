using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[ControllerName("Profile")]
[Route("/api/account/my-profile")]
public class ProfileController(IProfileAppService profileAppService) : AbpControllerBase, IProfileAppService
{
    protected IProfileAppService ProfileAppService { get; } = profileAppService;

    [HttpGet]
    public virtual Task<ProfileDto> GetAsync() => ProfileAppService.GetAsync();

    [HttpPut]
    public virtual Task<ProfileDto> UpdateAsync(UpdateProfileDto input) => ProfileAppService.UpdateAsync(input);

    [HttpPost]
    [Route("change-password")]
    public virtual Task ChangePasswordAsync(ChangePasswordInput input) => ProfileAppService.ChangePasswordAsync(input);
}
