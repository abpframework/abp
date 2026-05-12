```json
//[doc-seo]
{
    "Description": "Learn how authentication and authorization are configured in ABP React UI applications."
}
```

# Authorization in React UI

OAuth is preconfigured in ABP React UI templates. When you create a React solution with ABP Studio v3.0+ or `abp new --modern --ui-framework react`, the template includes OpenID Connect settings, an OpenIddict client, route guards, and authentication hooks.

The React app authenticates against the ABP Auth Server using the **Authorization Code flow with PKCE**, which is the recommended flow for browser-based applications.

## Packages

The template uses these packages for authentication:

| Package | Purpose |
| --- | --- |
| `@volo/abp-oidc-auth` | Framework-agnostic OIDC client helpers for ABP/OpenIddict backends. |
| `@volo/abp-react-oidc-auth` | React adapter for the ABP OIDC client. |
| `oidc-client-ts` | Underlying OIDC protocol implementation. |

The package list also includes `@volo/abp-app-config` and `@volo/abp-react-app-config`, which are used to fetch application configuration and permissions after authentication.

## OAuth Configuration

The OIDC settings are resolved from runtime configuration first and fall back to `src/env.ts`.

```ts
export function getOAuthConfig(): {
  issuer: string
  redirectUri: string
  clientId: string
  scope: string
  responseType: 'code'
} {
  return {
    issuer: loadedConfig?.oAuthConfig?.issuer ?? env.oauth.issuer,
    redirectUri: loadedConfig?.oAuthConfig?.redirectUri ?? env.oauth.redirectUri,
    clientId: loadedConfig?.oAuthConfig?.clientId ?? env.oauth.clientId,
    scope: loadedConfig?.oAuthConfig?.scope ?? env.oauth.scope,
    responseType: 'code',
  }
}
```

The important configuration values are:

- `oAuthConfig.issuer`: Auth Server / OpenIddict authority URL.
- `oAuthConfig.redirectUri`: URL where the Auth Server redirects after login.
- `oAuthConfig.clientId`: OpenIddict client ID, normally `<ProjectName>_App`.
- `oAuthConfig.scope`: Scopes requested by the React app.

See [Environment Variables](./environment-variables.md) for the full runtime configuration model.

## Initializing Authentication

The app loads runtime configuration before initializing OIDC:

```tsx
async function bootstrap() {
  await loadRuntimeConfig()
  initUserManager()
  createRoot(document.getElementById('root')!).render(
    <StrictMode>
      <App />
    </StrictMode>
  )
}
```

`initUserManager()` creates the ABP React OIDC client:

```ts
client = createAbpReactOidcAuth({
  authority: config.issuer,
  clientId: config.clientId,
  redirectUri: config.redirectUri,
  postLogoutRedirectUri: config.redirectUri,
  scope: config.scope,
  responseType: config.responseType,
  automaticSilentRenew: true,
  userStoreType: 'localStorage',
  userStorePrefix: `oidc.${config.clientId}`,
  silentRedirectUri: `${window.location.origin}/silent-renew.html`,
})
```

The template stores the OIDC user in local storage and enables silent renewal with `public/silent-renew.html`.

## Auth Provider and Hook

`AuthProvider` wraps the app and handles the OIDC callback:

```tsx
export function AuthProvider({ children }: { children: ReactNode }) {
  const authClient = getAuthClient()

  useEffect(() => {
    const params = new URLSearchParams(window.location.search)
    if (!params.has('code') || !params.has('state')) return
    void authClient.handleSigninCallback().then(() =>
      window.history.replaceState({}, document.title, window.location.pathname)
    )
  }, [])

  return <authClient.AuthProvider>{children}</authClient.AuthProvider>
}
```

Use `useAuth()` in components:

```tsx
import { useAuth } from '@/lib/auth/AuthContext'

export function LoginButton() {
  const { isAuthenticated, isLoading, login, logout, user } = useAuth()

  if (isLoading) return null

  return isAuthenticated ? (
    <button onClick={() => void logout()}>{user?.name ?? 'Logout'}</button>
  ) : (
    <button onClick={() => void login()}>Login</button>
  )
}
```

## Route Protection

The React template uses TanStack Router. Protected routes use `beforeLoad` guards.

```ts
const identityLayoutRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/identity',
  component: IdentityLayout,
  beforeLoad: authGuard,
})
```

`authGuard` checks the current OIDC user and redirects unauthenticated users to the Auth Server:

```ts
export async function authGuard({ location }: GuardContext) {
  const user = await userManager.getUser()
  if (!user || user.expired) {
    await userManager.signinRedirect({
      state: { returnUrl: location.href },
    })
    throw new Error('Redirecting to login')
  }
}
```

Routes that also require a permission use `createPermissionGuard`:

```ts
const usersRoute = createRoute({
  getParentRoute: () => identityLayoutRoute,
  path: 'users',
  component: UsersPage,
  beforeLoad: createPermissionGuard('AbpIdentity.Users'),
})
```

Permission checks are explained in [Permission Management](./permission-management.md).

## OpenIddict Clients

The generated OpenIddict clients depend on the template:

- Layered and single-layer modern templates use the main React client, normally `<ProjectName>_App`.
- Microservice modern templates also include an Admin Console client, normally `<ProjectName>_AdminConsole`, because the Admin Console is a separate React app.

If you change URLs after generation, update both the runtime configuration and the corresponding OpenIddict client redirect URLs.

## See Also

- [Environment Variables](./environment-variables.md)
- [Permission Management](./permission-management.md)
- [Authorization](../../../framework/fundamentals/authorization/index.md)
