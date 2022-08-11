using System.Net.Http;

namespace Volo.Abp.AspNetCore.Authentication.OpenIdConnect;

public class OpenIdLocalUserCreationClientOptions
{
    /// <summary>
    /// Can be used to enable/disable request to the server to create/update local users.
    /// Default value: false
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Default value: "AbpIdentity".
    /// Fallbacks to the "Default" remote service configuration, if "AbpIdentity" configuration is not available.
    /// Set to null if you don't want to use a remote service configuration. In this case, you can set an
    /// absolute URL in the <see cref="Url"/> option.
    /// </summary>
    public string RemoteServiceName { get; set; } = "AbpIdentity";

    /// <summary>
    /// URL to make a POST request after the current user successfully authenticated through an OpenIdConnect provider.
    /// </summary>
    public string Url { get; set; } = "/api/identity-profile/create-or-update";
    
    /// <summary>
    /// Can be set to a value if you want to use a named <see cref="HttpClient"/> instance
    /// while creating it from <see cref="IHttpClientFactory"/>.
    /// Default value: "" (<see cref="Microsoft.Extensions.Options.Options.DefaultName"/>).
    /// </summary>
    public string HttpClientName { get; } = Microsoft.Extensions.Options.Options.DefaultName;
}