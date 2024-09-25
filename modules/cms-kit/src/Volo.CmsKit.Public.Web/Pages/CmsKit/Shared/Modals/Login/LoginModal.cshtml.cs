using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Modals.Login;

public class LoginModalModel : CmsKitPublicPageModelBase
{
    public AbpMvcUiOptions AbpMvcUiOptions { get; }

    public LoginModalViewModel ViewModel { get; set; }
    public LoginModalModel(IOptions<AbpMvcUiOptions> abpMvcUiOptions)
    {
        AbpMvcUiOptions = abpMvcUiOptions.Value;
    }
    public void OnGet(string message, string? returnUrl = "")
    {
        ViewModel = new LoginModalViewModel
        {
            LoginUrl = $"{AbpMvcUiOptions.LoginUrl}?returnUrl={returnUrl}",
            Message = message
        };
    }
}

public class LoginModalViewModel
{
    [NotNull]
    public string Message { get; set; }
    
    public string LoginUrl {  get; set; }
}