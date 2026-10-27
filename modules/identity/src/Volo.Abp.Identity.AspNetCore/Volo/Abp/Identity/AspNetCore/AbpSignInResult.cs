using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity.AspNetCore;

public class AbpSignInResult : SignInResult
{
    /// <summary>
    /// The user is authenticated by an external login provider but has to change the password,
    /// which can't be done with the password of the external login provider.
    /// </summary>
    public static AbpSignInResult ExternalLoginPasswordChangeRequired { get; } = new AbpSignInResult
    {
        IsNotAllowed = true,
        IsExternalLoginPasswordChangeRequired = true
    };

    public bool IsExternalLoginPasswordChangeRequired { get; protected set; }

    public override string ToString()
    {
        return IsExternalLoginPasswordChangeRequired
            ? nameof(ExternalLoginPasswordChangeRequired)
            : base.ToString();
    }
}
