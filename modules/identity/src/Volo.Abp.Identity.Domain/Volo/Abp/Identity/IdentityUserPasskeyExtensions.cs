using Microsoft.AspNetCore.Identity;

namespace Volo.Abp.Identity;

public static class IdentityUserPasskeyExtensions
{
    public static void UpdateFromUserPasskeyInfo(this IdentityUserPasskey passkey, UserPasskeyInfo passkeyInfo)
    {
        passkey.Data.Name = passkeyInfo.Name;
        passkey.Data.SignCount = passkeyInfo.SignCount;
        passkey.Data.IsBackedUp = passkeyInfo.IsBackedUp;
        passkey.Data.IsUserVerified = passkeyInfo.IsUserVerified;
    }

    public static UserPasskeyInfo ToUserPasskeyInfo(this IdentityUserPasskey passkey)
    {
        return new UserPasskeyInfo(
            passkey.CredentialId,
            passkey.Data.PublicKey,
            passkey.Data.CreatedAt,
            passkey.Data.SignCount,
            passkey.Data.Transports,
            passkey.Data.IsUserVerified,
            passkey.Data.IsBackupEligible,
            passkey.Data.IsBackedUp,
            passkey.Data.AttestationObject,
            passkey.Data.ClientDataJson)
        {
            Name = passkey.Data.Name
        };
    }
}
