namespace Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

public class PersistentAccessToken
{
    public const string Key = "access_token";

    public string? AccessToken { get; set; }
}
