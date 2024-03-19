using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer.DynamicClaims;

public class WebRemoteDynamicClaimsPrincipalContributorOptions
{
    public bool IsEnabled { get; set; }

    public string AuthenticationScheme { get; set; }

    public WebRemoteDynamicClaimsPrincipalContributorOptions()
    {
        IsEnabled = false;
        AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}
