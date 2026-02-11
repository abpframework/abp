namespace Volo.Abp.AspNetCore.Components.Web;

public class AbpAuthenticationOptions
{
    public string LoginUrl { get; set; } = "Account/Login";

    public string LogoutUrl { get; set; } = "Account/Logout"; 
}