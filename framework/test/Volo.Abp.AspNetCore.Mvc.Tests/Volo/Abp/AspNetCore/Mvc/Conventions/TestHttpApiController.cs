using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

[RemoteService(Name = "test")]
[Area("test")]
[ControllerName("test")]
[Route("api/identity/roles")]
public class TestHttpApiController : AbpControllerBase, IApplicationService
{
    [HttpDelete]
    [Route("{id}/assign/{assignToId?}")]
    public virtual Task DeleteAsync(Guid id, Guid? assignToId)
    {
        return Task.CompletedTask;
    }
}
