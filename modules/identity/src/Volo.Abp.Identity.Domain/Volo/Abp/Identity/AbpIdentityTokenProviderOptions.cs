namespace Volo.Abp.Identity;

public class AbpIdentityTokenProviderOptions
{
    /// <summary>
    /// Has to be set with <c>PreConfigure</c>: it is read while the token providers are being registered.
    /// </summary>
    public bool UseAbpTokenProviders { get; set; } = true;
}
