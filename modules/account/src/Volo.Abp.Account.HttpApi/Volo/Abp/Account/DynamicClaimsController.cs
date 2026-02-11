using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[ControllerName("DynamicClaims")]
[Route("/api/account/dynamic-claims")]
public class DynamicClaimsController : AbpControllerBase, IDynamicClaimsAppService
{
    protected IDynamicClaimsAppService DynamicClaimsAppService { get; }

    public DynamicClaimsController(IDynamicClaimsAppService dynamicClaimsAppService)
    {
        DynamicClaimsAppService = dynamicClaimsAppService;
    }

    [HttpPost]
    [Route("refresh")]
    public virtual Task RefreshAsync()
    {
        return DynamicClaimsAppService.RefreshAsync();
    }
}
