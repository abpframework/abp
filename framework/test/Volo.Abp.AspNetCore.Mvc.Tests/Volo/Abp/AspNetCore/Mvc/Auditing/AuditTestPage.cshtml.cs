using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Auditing;

namespace Volo.Abp.AspNetCore.Mvc.Auditing;

public class AuditTestPage : AbpPageModel
{
    private readonly AbpAuditingOptions _options;

    public AuditTestPage(IOptions<AbpAuditingOptions> options)
    {
        _options = options.Value;
    }

    public void OnGet()
    {

    }

    public IActionResult OnGetAuditSuccessForGetRequests()
    {
        return new OkResult();
    }

    public IActionResult OnGetAuditFailForGetRequests()
    {
        throw new UserFriendlyException("Exception occurred!");
    }

    public ObjectResult OnGetAuditFailForGetRequestsReturningObject()
    {
        throw new UserFriendlyException("Exception occurred!");
    }

    public IActionResult OnGetAuditActivateFailed([FromServices] AbpAuditingOptions options)
    {
        return new OkResult();
    }
}
