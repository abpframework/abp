namespace Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme;

public class AuthenticationOptions
{
    public string LoginUrl { get; set; } = "authentication/login";

    public string LogoutUrl { get; set; } = "authentication/logout";
}
