using JetBrains.Annotations;

namespace Volo.Abp.Identity;

public class ExternalLoginUserInfo
{
    [CanBeNull]
    public string Name { get; set; }

    [CanBeNull]
    public string Surname { get; set; }

    [CanBeNull]
    public string PhoneNumber { get; set; }

    [NotNull]
    public string Email { get; private set; }

    [CanBeNull]
    public bool? PhoneNumberConfirmed { get; set; }

    [CanBeNull]
    public bool? EmailConfirmed { get; set; }

    [CanBeNull]
    public bool? TwoFactorEnabled { get; set; }

    [CanBeNull]
    public string ProviderKey { get; set; }

    public ExternalLoginUserInfo([NotNull] string email)
    {
        Email = Check.NotNullOrWhiteSpace(email, nameof(email));
    }
}
