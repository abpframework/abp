using Microsoft.AspNetCore.Identity;
using Volo.Abp.Identity;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpIdentityBuilderExtensions
{
    /// <summary>
    /// The providers are written for <see cref="IdentityUser"/>, so the builder has to be one for that
    /// user type.
    /// </summary>
    public static IdentityBuilder AddAbpTokenProviders(this IdentityBuilder builder)
    {
        builder
            .AddTokenProvider<AbpDefaultTokenProvider>(TokenOptions.DefaultProvider)
            .AddTokenProvider<AbpEmailTwoFactorTokenProvider>(TokenOptions.DefaultEmailProvider)
            .AddTokenProvider<AbpPhoneNumberTwoFactorTokenProvider>(TokenOptions.DefaultPhoneProvider)
            .AddTokenProvider<AuthenticatorTokenProvider<IdentityUser>>(TokenOptions.DefaultAuthenticatorProvider)
            .AddTokenProvider<AbpPasswordResetTokenProvider>(AbpPasswordResetTokenProvider.ProviderName)
            .AddTokenProvider<AbpEmailConfirmationTokenProvider>(AbpEmailConfirmationTokenProvider.ProviderName)
            .AddTokenProvider<AbpChangeEmailTokenProvider>(AbpChangeEmailTokenProvider.ProviderName)
            .AddTokenProvider<LinkUserTokenProvider>(LinkUserTokenProviderConsts.LinkUserTokenProviderName);

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Tokens.PasswordResetTokenProvider = AbpPasswordResetTokenProvider.ProviderName;
            options.Tokens.EmailConfirmationTokenProvider = AbpEmailConfirmationTokenProvider.ProviderName;
            options.Tokens.ChangeEmailTokenProvider = AbpChangeEmailTokenProvider.ProviderName;
        });

        return builder;
    }
}
