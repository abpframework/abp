using System.Collections.Generic;
using System.Threading.Tasks;
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

    [HttpGet]
    public virtual Task<List<DynamicClaimDto>> GetAsync()
    {
        return DynamicClaimsAppService.GetAsync();
    }
}
