using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Views.Error;

public class AbpErrorViewModel
{
    public ErrorInfo ErrorInfo { get; set; }

    public int HttpStatusCode { get; set; }
}
