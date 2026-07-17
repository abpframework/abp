```json
//[doc-seo]
{
    "Description": "Explore the ABP OAuth Package to learn about authentication integration in your app, including essential classes and functions for seamless user management."
}
```

# ABP OAuth Package

The authentication implementation was moved from `@abp/ng.core` to `@abp/ng.oauth` in v7.0. The core package defines the authentication abstractions and tokens, while the OAuth package supplies their `angular-oauth2-oidc` implementations.

The package is included in the Angular application templates. Install it if the application does not already reference it:

```bash
npm install @abp/ng.oauth
```

## Standalone Setup

Add `provideAbpOAuth()` after `provideAbpCore()` in the `providers` array of `app.config.ts`:

```ts
import { ApplicationConfig } from '@angular/core';
import { provideAbpCore, withOptions } from '@abp/ng.core';
import { registerLocaleForEsBuild } from '@abp/ng.core/locale';
import { provideAbpOAuth } from '@abp/ng.oauth';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocaleForEsBuild(),
      }),
    ),
    provideAbpOAuth(),
  ],
};
```

`AbpOAuthModule.forRoot()` is deprecated. Use the standalone provider for new applications.

## Registered Authentication Services

`provideAbpOAuth()` registers or replaces these public core abstractions:

- `AuthService`, `AuthGuard`, `authGuard` and `asyncAuthGuard` with their OAuth implementations.
- `ApiInterceptor` and its `HTTP_INTERCEPTORS` registration.
- `PIPE_TO_LOGIN_FN_KEY` with the function used when authentication is required.
- `CHECK_AUTHENTICATION_STATE_FN_KEY` with the function that checks and stores the current authentication state.
- `NAVIGATE_TO_MANAGE_PROFILE` with navigation to the authority's account-management page.
- `AuthErrorFilterService` with the OAuth error filter.
- `OAuthStorage`, using `BrowserTokenStorageService` or `ServerTokenStorageService` for an SSR-started application and `MemoryTokenStorageService` otherwise.

It also registers the OAuth configuration initializer and the providers from `angular-oauth2-oidc`.

## API Interceptor

For non-external requests, the OAuth API interceptor adds an `Authorization` bearer token, `Accept-Language`, the configured tenant header and `X-Requested-With` when the corresponding values are available. Existing authorization, language and tenant headers are preserved. Requests marked with the `IS_EXTERNAL_REQUEST` HTTP context token are sent without those ABP headers. The interceptor also integrates every request with the HTTP wait service.

To implement another authentication system, provide replacements for the core services and tokens used by the application instead of depending on the OAuth implementations.
