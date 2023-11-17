using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Volo.Abp.AspNetCore.Authentication.JwtBearer.DynamicClaims;

public class WebRemoteDynamicClaimsPrincipalContributorOptions
{
    public string AuthenticationScheme { get; set; }

    public WebRemoteDynamicClaimsPrincipalContributorOptions()
    {
        AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}
