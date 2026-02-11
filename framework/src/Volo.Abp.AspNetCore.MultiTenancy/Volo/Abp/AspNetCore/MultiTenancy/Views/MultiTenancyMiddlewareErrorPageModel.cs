namespace Volo.Abp.AspNetCore.MultiTenancy.Views;

public class MultiTenancyMiddlewareErrorPageModel
{
    public string Message { get; set; }

    public string Details { get; set; }

    public MultiTenancyMiddlewareErrorPageModel(string message, string details)
    {
        Message = message;
        Details = details;
    }
}
