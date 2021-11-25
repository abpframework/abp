namespace Volo.Abp.AspNetCore.Mvc.UI;

public class AbpMvcUiOptions
{
    /// <summary>
    /// Default value: "/Account/Login".
    /// </summary>
    public string LoginUrl { get; set; } = "/Account/Login";

    /// <summary>
    /// Default value: "/Account/Logout".
    /// </summary>
    public string LogoutUrl { get; set; } = "/Account/Logout";
}
