```json
//[doc-seo]
{
    "Description": "Learn how the ABP Identity token providers work, what each of them is used for, and how to configure, extend or replace them."
}
```

# Identity Token Providers

ASP.NET Core Identity uses `IUserTwoFactorTokenProvider<TUser>` to issue and validate the one-off tokens behind password reset, email confirmation, change email, two-factor codes and similar flows.

ABP registers its own providers for these keys. They are single-active: generating a token for the same `(user, provider, purpose)` invalidates the previously issued one, and a two-factor code is consumed the moment it verifies. Lifespans are set per use case rather than shared, and a stored token can be revoked without rotating the user's `SecurityStamp`. The `Authenticator` key keeps ASP.NET Core's provider, because authenticator apps require TOTP. `AbpIdentityDomainModule` does the registration, so every host that loads it resolves the same providers.

## Built-in Providers

| Provider key | Provider | Default | Used by |
| --- | --- | --- | --- |
| `TokenOptions.DefaultProvider` (`"Default"`) | `AbpDefaultTokenProvider` | 10 minutes | Generic challenge tokens (e.g. `RequiresTwoFactor`, `ShouldChangePasswordOnNextLogin`, `PeriodicallyChangePassword`) issued by IdentityServer / OpenIddict password flow endpoints |
| `AbpPasswordResetTokenProvider.ProviderName` (`"AbpPasswordReset"`) | `AbpPasswordResetTokenProvider` | 2 hours | `UserManager.GeneratePasswordResetTokenAsync` / `ResetPasswordAsync` |
| `AbpEmailConfirmationTokenProvider.ProviderName` (`"AbpEmailConfirmation"`) | `AbpEmailConfirmationTokenProvider` | 2 hours | `UserManager.GenerateEmailConfirmationTokenAsync` / `ConfirmEmailAsync` |
| `AbpChangeEmailTokenProvider.ProviderName` (`"AbpChangeEmail"`) | `AbpChangeEmailTokenProvider` | 2 hours | `UserManager.GenerateChangeEmailTokenAsync` / `ChangeEmailAsync` |
| `LinkUserTokenProviderConsts.LinkUserTokenProviderName` (`"AbpLinkUser"`) | `LinkUserTokenProvider` | 10 minutes | `IdentityLinkUserManager.GenerateLinkTokenAsync` / `VerifyLinkTokenAsync` for cross-tenant account linking |
| `TokenOptions.DefaultEmailProvider` (`"Email"`) | `AbpEmailTwoFactorTokenProvider` | 3 minutes | 6-digit numeric 2FA code delivered by email |
| `TokenOptions.DefaultPhoneProvider` (`"Phone"`) | `AbpPhoneNumberTwoFactorTokenProvider` | 3 minutes | 6-digit numeric 2FA code delivered by SMS, also used by `UserManager.GenerateChangePhoneNumberTokenAsync` |
| `TokenOptions.DefaultAuthenticatorProvider` (`"Authenticator"`) | ASP.NET Core's built-in `AuthenticatorTokenProvider<TUser>`, unless another module replaces it | TOTP timestep | Authenticator-app TOTP per [RFC 6238](https://datatracker.ietf.org/doc/html/rfc6238) |

`IdentityOptions.Tokens.PasswordResetTokenProvider`, `EmailConfirmationTokenProvider`, and `ChangeEmailTokenProvider` are redirected by ABP to the dedicated single-active providers above. `ChangePhoneNumberTokenProvider` keeps its ASP.NET Core default of `"Phone"`, so it shares the 2FA phone provider's 6-digit-code semantics rather than going through the DataProtector pipeline.

## How Single-Active Tokens Work

The DataProtector-based providers (`AbpDefaultTokenProvider`, `AbpPasswordResetTokenProvider`, `AbpEmailConfirmationTokenProvider`, `AbpChangeEmailTokenProvider`, `LinkUserTokenProvider`) all derive from the abstract `AbpSingleActiveTokenProvider`. It protects a payload carrying the user id, purpose, `SecurityStamp` and a creation timestamp with the Data Protection purpose taken from the options `Name`, and adds a stored-hash check on top:

1. **Generation**: the provider produces the protected token blob. The provider then computes `SHA-256(token)` and stores its hex string as a user token under the login provider `"[AbpSingleActiveToken]"` and the name `"<ProviderName>:<purpose>"`. Generating a new token overwrites the same entry, so the previous token's stored hash no longer matches.
2. **Validation**: after the protected blob has been accepted (`DataProtector` unprotect, lifespan, user id, purpose and `SecurityStamp` checks), the stored hash is loaded and compared against `SHA-256(submitted token)` using `CryptographicOperations.FixedTimeEquals`. If no hash exists, the token is rejected. A non-hex stored value is treated as invalid rather than thrown.

This has the following effects:

- **Single active token**: generating one invalidates the previous token for the same `(user, provider, purpose)`. Multiple requests in flight will only let the most recent one complete.
- **Per-purpose isolation**: the stored hash key includes the purpose, so a `RequiresTwoFactor` token and a `ShouldChangePasswordOnNextLogin` token issued under the same `"Default"` provider do not invalidate each other.
- **`SecurityStamp` rotation invalidates every issued token**: the stamp is part of the protected payload and is compared on validation. The numeric 2FA codes do not carry it and are not affected by rotating it.
- **Validation never throws on data corruption**: a non-hex stored hash returns `false` from `ValidateAsync` instead of propagating a `FormatException`.
- **The stored hash and the key ring are shared state**: validating a token means reading the hash back from the same database, and the same tenant database when the solution keeps one per tenant, and unprotecting the payload with the same Data Protection key ring and `SetApplicationName` value. Generating one writes the hash, so that side needs write access and its transaction has to commit before the token is used.

The 2FA OTP providers (`AbpEmailTwoFactorTokenProvider`, `AbpPhoneNumberTwoFactorTokenProvider`) use a different mechanism. See [Two Factor Authentication](./two-factor-authentication.md#how-the-verification-code-is-generated) for the numeric-code single-use design.

## Configuring the Providers

Each DataProtector-based provider exposes an options class deriving from `AbpDataProtectionTokenProviderOptions`, configurable through the standard [options pattern](../../framework/fundamentals/options.md):

| Options class | Default | Used by |
| --- | --- | --- |
| `AbpDefaultTokenProviderOptions` | 10 minutes | Generic challenge tokens (login flow) |
| `AbpPasswordResetTokenProviderOptions` | 2 hours | Password reset links |
| `AbpEmailConfirmationTokenProviderOptions` | 2 hours | Email confirmation links |
| `AbpChangeEmailTokenProviderOptions` | 2 hours | Change-email confirmation links |
| `AbpLinkUserTokenProviderOptions` | 10 minutes | Cross-tenant account linking |

Override them in your module's `ConfigureServices`:

```csharp
Configure<AbpDefaultTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(15);
});

Configure<AbpPasswordResetTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(1);
});
```

The `Name` property is set by the constructor of each options class and defaults to the key the provider is registered under. It is the Data Protection purpose the token is protected with and the prefix of its stored hash, so changing it invalidates every outstanding token and every host that validates one has to be given the same value. It does not move the provider: the key in `IdentityOptions.Tokens.ProviderMap` is fixed when the provider is registered.

Expiration is checked when a token is validated, so the lifespan has to be configured wherever that happens.

For OTP-based options see [Configuring the Default Providers](./two-factor-authentication.md#configuring-the-default-providers) in the 2FA document.

## Disabling the ABP Token Providers

To take the token providers into your own hands, turn the ABP ones off and register what you want instead:

```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpIdentityTokenProviderOptions>(options =>
    {
        options.UseAbpTokenProviders = false;
    });

    PreConfigure<IdentityBuilder>(builder =>
    {
        builder.AddDefaultTokenProviders();
    });
}
```

Both have to be set with `PreConfigure`, not `Configure` — the registration reads the pre-configured actions while the services are still being registered. Apply the same configuration on **every** host that generates or validates a token, because a token is bound to the provider that produced it.

The flag turns off the registration itself, not only ABP's choice of provider: the Identity module then registers no token provider at all, and a flow whose key has no provider throws a `NotSupportedException` on the first call. `AddDefaultTokenProviders()` is the ASP.NET Core registration and covers the `Default`, `Email`, `Phone` and `Authenticator` keys. Nothing is registered for the `AbpLinkUser` key, so a solution that links accounts has to provide one for it either way. Other modules keep whatever they register themselves.

## Invalidating a Stored Token

To force a stored single-active token to become invalid before its natural expiration (for example after a security-relevant action), call one of the `IdentityUserManagerSingleActiveTokenExtensions` helpers:

```csharp
await UserManager.RemovePasswordResetTokenAsync(user);
await UserManager.RemoveEmailConfirmationTokenAsync(user);
await UserManager.RemoveChangeEmailTokenAsync(user, newEmail);
await UserManager.RemoveLinkUserTokenAsync(user);
await UserManager.RemoveLinkUserTokenAsync(user, customPurpose);
```

Each method removes the stored hash under `"[AbpSingleActiveToken]"` for the corresponding purpose. Validation afterwards returns `false` even if the token blob itself is still within its DataProtector lifespan and the `SecurityStamp` is unchanged. They throw an `AbpException` when the key is not served by an `AbpSingleActiveTokenProvider`: with the ABP providers turned off there is no stored hash to remove, and a token that was never single-active cannot be revoked this way.

For tokens issued by `AbpDefaultTokenProvider` (e.g. `RequiresTwoFactor`, `ShouldChangePasswordOnNextLogin`, `PeriodicallyChangePassword`), call `UserManager.RemoveAuthenticationTokenAsync` directly. The name is built from the provider's options `Name`, which is `TokenOptions.DefaultProvider` unless you changed it:

```csharp
await UserManager.RemoveAuthenticationTokenAsync(
    user,
    AbpSingleActiveTokenProvider.InternalLoginProvider,
    TokenOptions.DefaultProvider + ":" + nameof(SignInResult.RequiresTwoFactor));
```

## Replacing a Provider

If the built-in behavior does not match your requirements (different storage backend, different lifespan policy, alphanumeric codes, etc.), register your own implementation under the same key. `IdentityBuilder.AddTokenProvider` writes to `IdentityOptions.Tokens.ProviderMap` and the last registration wins:

```csharp
PreConfigure<IdentityBuilder>(builder =>
{
    builder.AddTokenProvider<MyDefaultTokenProvider>(TokenOptions.DefaultProvider);
    builder.AddTokenProvider<MyPasswordResetTokenProvider>(AbpPasswordResetTokenProvider.ProviderName);
});
```

The most ergonomic starting point for a single-active variant is to subclass `AbpSingleActiveTokenProvider` and supply your own options class deriving from `AbpDataProtectionTokenProviderOptions`. Give it a `Name` of its own in the constructor, the way the built-in options classes do: the inherited default is a shared one, and two providers under the same name share a Data Protection purpose and a stored-hash key. For a numeric-code provider, subclass `AbpTwoFactorTokenProvider` instead. See the [Two Factor Authentication](./two-factor-authentication.md#replacing-the-verification-code-provider) document.

## Behavioral Notes

- **A single key can be taken over on its own**: registering another provider under one key leaves the rest on the ABP ones, see [Replacing a Provider](#replacing-a-provider). Use `AbpIdentityTokenProviderOptions.UseAbpTokenProviders` to take over all of them at once, see [Disabling the ABP Token Providers](#disabling-the-abp-token-providers).
- **Stored entries are per-tenant**: the single-active hashes are persisted as `IdentityUserToken` records, which carry the user's `TenantId`. They are not shared across tenants.
- **Cleanup behavior**: `Remove*TokenAsync` helpers delete the stored hash entry directly. Generating a new token under the same `(user, provider, purpose)` overwrites the existing entry. DataProtector-based tokens, unlike 2FA OTP codes, are not consumed on successful verification. The stored hash remains until a new token is issued or the entry is explicitly removed.
- **Custom purposes work transparently**: a call like `GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "MyCustomPurpose")` goes through `AbpDefaultTokenProvider` and gets single-active semantics for `(user, "Default", "MyCustomPurpose")` automatically. The same applies to any custom token provider you register that subclasses `AbpSingleActiveTokenProvider`.
