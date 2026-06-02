```json
//[doc-seo]
{
    "Description": "Learn how ABP Identity replaces the ASP.NET Core Identity built-in token providers with single-active variants, what each provider is used for, and how to configure or replace them."
}
```

# Identity Token Providers

ASP.NET Core Identity uses `IUserTwoFactorTokenProvider<TUser>` to issue and validate one-off tokens such as password reset, email confirmation, change email, two-factor codes, and so on. The default registrations (`DataProtectorTokenProvider<TUser>` and the TOTP-based `EmailTokenProvider<TUser>` / `PhoneNumberTokenProvider<TUser>`) are general-purpose: tokens stay valid for the full configured lifespan and are not invalidated when a new token is issued.

ABP replaces the `Default`, `Email`, and `Phone` provider registrations with single-active variants, and redirects `IdentityOptions.Tokens.PasswordResetTokenProvider` / `EmailConfirmationTokenProvider` / `ChangeEmailTokenProvider` to dedicated single-active providers. Generating a new token for the same `(user, provider, purpose)` invalidates the previously issued one, and tokens for the DataProtector-based providers are short-lived by default. The `Authenticator` provider is left as-is because authenticator apps require TOTP. The replacements are wired up in `AbpIdentityAspNetCoreModule.PreConfigureServices`.

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
| `TokenOptions.DefaultAuthenticatorProvider` (`"Authenticator"`) | ASP.NET Core's built-in `AuthenticatorTokenProvider<TUser>` | TOTP timestep | Authenticator-app TOTP per [RFC 6238](https://datatracker.ietf.org/doc/html/rfc6238) |

`IdentityOptions.Tokens.PasswordResetTokenProvider`, `EmailConfirmationTokenProvider`, and `ChangeEmailTokenProvider` are redirected by ABP to the dedicated single-active providers above. `ChangePhoneNumberTokenProvider` keeps its ASP.NET Core default of `"Phone"`, so it shares the 2FA phone provider's 6-digit-code semantics rather than going through the DataProtector pipeline.

## How ABP Token Providers Differ from the Defaults

The default `DataProtectorTokenProvider<TUser>` creates a protected token blob containing the user id, purpose, security stamp and a creation timestamp. Validation unprotects the blob, checks the security stamp, and compares the timestamp against `DataProtectionTokenProviderOptions.TokenLifespan` (1 day by default). No server-side state is kept, so older tokens stay valid in parallel and the only ways to revoke before expiration are rotating the user's `SecurityStamp` (which signs every session out) or waiting out the lifespan. One day is fine for an emailed reset link, but far too long for a login-time challenge token where the user is expected to complete the next step within minutes.

The default email and phone providers use TOTP-style 6-digit codes. A code can be used more than once during its short validity window (the implementation accepts the previous timestep as well, giving an effective 3–6 minute window), and requesting another code in the same window returns the same value, which is confusing for a user who requests a new code after a typo.

ABP changes these registrations to make the affected tokens single-active and to use shorter defaults where appropriate:

| Property | ASP.NET Core default | ABP replacement |
| --- | --- | --- |
| New token revokes the old one (same user/purpose) | ❌ Multiple tokens valid in parallel | ✅ Single-active |
| Lifespan tightened per use case | ❌ Same 1 day for every DataProtector token | ✅ 10 min – 2 h |
| Server-side revoke without rotating `SecurityStamp` | ❌ Not supported | ✅ `Remove*TokenAsync` helpers |
| 2FA code consumed on successful verification | ❌ Replayable within the validity window | ✅ Single-use |
| Re-issuing a 2FA code in the same window | ⚠️ Same code returned | ✅ New random code |

`SecurityStamp`-based invalidation still applies on top of the ABP variants: rotating a user's security stamp invalidates every issued token regardless of provider.

## How Single-Active Tokens Work

The DataProtector-based providers (`AbpDefaultTokenProvider`, `AbpPasswordResetTokenProvider`, `AbpEmailConfirmationTokenProvider`, `AbpChangeEmailTokenProvider`, `LinkUserTokenProvider`) all derive from the abstract `AbpSingleActiveTokenProvider`, which itself extends ASP.NET Core's `DataProtectorTokenProvider<IdentityUser>`. On top of the base provider it adds a stored-hash check:

1. **Generation.** The base provider produces the protected token blob as usual. The provider then computes `SHA-256(token)` and stores its hex string in the user-token table under the login provider `"[AbpSingleActiveToken]"` and the name `"<ProviderName>:<purpose>"`. Generating a new token overwrites the same entry, so the previous token's stored hash no longer matches.
2. **Validation.** After the base provider has accepted the token (`SecurityStamp` and `DataProtector` checks), the stored hash is loaded and compared against `SHA-256(submitted token)` using `CryptographicOperations.FixedTimeEquals`. If no hash exists, the token is rejected. A non-hex stored value is treated as invalid rather than thrown.

This has the following effects:

- **Generating a new token invalidates the previous one** for the same `(user, provider, purpose)`. Multiple requests in flight will only let the most recent token complete.
- **Per-purpose isolation.** The stored hash key includes the purpose, so a `RequiresTwoFactor` token and a `ShouldChangePasswordOnNextLogin` token issued under the same `"Default"` provider do not invalidate each other.
- **`SecurityStamp` rotation invalidates every issued token.** This is inherited from the base `DataProtectorTokenProvider` and is unchanged.
- **Validation never throws on data corruption.** A non-hex stored hash returns `false` from `ValidateAsync` instead of propagating a `FormatException`.

The 2FA OTP providers (`AbpEmailTwoFactorTokenProvider`, `AbpPhoneNumberTwoFactorTokenProvider`) use a different mechanism — see [Two Factor Authentication](./two-factor-authentication.md#how-the-verification-code-is-generated) for the numeric-code single-use design.

## Configuring the Providers

Each DataProtector-based provider exposes an options class deriving from `DataProtectionTokenProviderOptions`, configurable through the standard [options pattern](../../framework/fundamentals/options.md):

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

The `Name` property is set by the constructor of each options class and should not normally be changed — it is the same key that the provider is registered under in `IdentityOptions.Tokens.ProviderMap`.

For OTP-based options see [Configuring the Default Providers](./two-factor-authentication.md#configuring-the-default-providers) in the 2FA document.

## Invalidating a Stored Token

To force a stored single-active token to become invalid before its natural expiration (for example after a security-relevant action), call one of the `IdentityUserManagerSingleActiveTokenExtensions` helpers:

```csharp
await UserManager.RemovePasswordResetTokenAsync(user);
await UserManager.RemoveEmailConfirmationTokenAsync(user);
await UserManager.RemoveChangeEmailTokenAsync(user, newEmail);
await UserManager.RemoveLinkUserTokenAsync(user);
await UserManager.RemoveLinkUserTokenAsync(user, customPurpose);
```

Each method removes the stored hash under `"[AbpSingleActiveToken]"` for the corresponding purpose. Validation afterwards returns `false` even if the token blob itself is still within its DataProtector lifespan and the `SecurityStamp` is unchanged.

For tokens issued by `AbpDefaultTokenProvider` (e.g. `RequiresTwoFactor`, `ShouldChangePasswordOnNextLogin`, `PeriodicallyChangePassword`), call `UserManager.RemoveAuthenticationTokenAsync` directly:

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

The most ergonomic starting point for a single-active variant is to subclass `AbpSingleActiveTokenProvider` and supply your own options class. For a numeric-code provider, subclass `AbpTwoFactorTokenProvider` instead — see the [Two Factor Authentication](./two-factor-authentication.md#replacing-the-verification-code-provider) document.

## Compatibility Notes

- **Tokens issued before the upgrade are rejected after the switch.** The ABP providers look for a stored entry that older tokens (and TOTP 2FA codes) do not have, so they fail validation. Users should request a new password reset link, email confirmation, or 2FA code after the upgrade.
- **Opt out by re-registering the provider key.** If you want the original ASP.NET Core behavior (multi-active, 1 day lifespan) for a specific key, register `DataProtectorTokenProvider<IdentityUser>` (or your own provider) under the same key after the ABP module has run. `AddTokenProvider` writes to `IdentityOptions.Tokens.ProviderMap` and the last registration wins.
- **Stored entries are per-tenant.** The single-active hashes are persisted as `IdentityUserToken` records, which carry the user's `TenantId`. They are not shared across tenants.
- **Cleanup behavior.** `Remove*TokenAsync` helpers delete the stored hash entry directly. Generating a new token under the same `(user, provider, purpose)` overwrites the existing entry. DataProtector-based tokens, unlike 2FA OTP codes, are not consumed on successful verification — the stored hash remains until a new token is issued or the entry is explicitly removed.
- **Custom purposes work transparently.** A call like `GenerateUserTokenAsync(user, TokenOptions.DefaultProvider, "MyCustomPurpose")` goes through `AbpDefaultTokenProvider` and gets single-active semantics for `(user, "Default", "MyCustomPurpose")` automatically. The same applies to any custom token provider you register that subclasses `AbpSingleActiveTokenProvider`.
