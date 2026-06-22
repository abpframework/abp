```json
//[doc-seo]
{
    "Description": "Learn how permissions are fetched, stored, checked, and applied in ABP React UI applications."
}
```

# Permission Management

ABP permissions are defined on the server side and are exposed to the React app through ABP application configuration. The React template uses those permissions to protect routes, hide sidebar items, and conditionally render UI actions.

For the server-side permission system, see [Authorization](../../../framework/fundamentals/authorization/index.md).

## Packages

The React template uses:

| Package | Purpose |
| --- | --- |
| `@volo/abp-app-config` | Framework-agnostic ABP application configuration client. |
| `@volo/abp-react-app-config` | React hooks and adapters for application configuration. |

The template creates a shared app configuration client in `src/lib/auth/permissions.ts`:

```ts
export const appConfig = createAbpReactAppConfig({
  baseUrl: () => getApiUrl(),
  includeLocalizationResources: false,
})
```

## Fetching Permissions

After the user logs in, `AuthProvider` fetches application configuration with the current access token:

```ts
const user = await authClient.getUserManager().getUser()
if (user && !user.expired) {
  await fetchAppConfig(user.access_token ?? null)
}
```

`fetchAppConfig` also sends the current tenant ID when one is selected:

```ts
export async function fetchAppConfig(token: string | null): Promise<void> {
  const headers: Record<string, string> = {}
  const tenantId = sessionStorage.getItem('abp_tenant_id')
  if (tenantId) headers.__tenant = tenantId
  await appConfig.fetchConfig(token, { headers })
}
```

The response includes the current user's granted policies. These are stored by the app configuration client and exposed to React components.

## Checking Permissions in Components

Use `usePermissions()` from `src/lib/auth/permissions.ts`:

```tsx
import { usePermissions } from '@/lib/auth/permissions'

export function BookActions() {
  const { isGranted } = usePermissions()

  return (
    <>
      {isGranted('MyProjectName.Books.Edit') && <button>Edit</button>}
      {isGranted('MyProjectName.Books.Delete') && <button>Delete</button>}
    </>
  )
}
```

The Books page uses this pattern for edit and delete actions:

```ts
const { isGranted } = usePermissions()
const canEdit = isGranted('MyProjectName.Books.Edit')
const canDelete = isGranted('MyProjectName.Books.Delete')
```

## Route Guards

Routes can require a permission by using `createPermissionGuard`:

```ts
const booksRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/books',
  component: BooksPage,
  beforeLoad: createPermissionGuard('MyProjectName.Books'),
})
```

`createPermissionGuard` runs the authentication guard first, fetches app configuration if needed, and redirects to `/403` when the required policy is not granted.

```ts
export function createPermissionGuard(requiredPolicy: string) {
  return async (context: GuardContext) => {
    await authGuard(context)

    if (!appConfig.getSnapshot()?.initialized) {
      const user = await userManager.getUser()
      await fetchAppConfig(user?.access_token ?? null)
    }

    if (!isPolicyGranted(requiredPolicy)) throw redirect({ to: '/403' })
  }
}
```

## Sidebar Visibility

The sidebar reads `routeConfig` and hides items that require missing permissions:

```ts
export const routeConfig: RouteConfigItem[] = [
  {
    path: '/identity/users',
    nameKey: 'AbpIdentity::Users',
    requiredPolicy: 'AbpIdentity.Users',
  },
]
```

The sidebar checks each item:

```ts
if (item.requiresAuth && !isAuthenticated) return false
if (!item.requiredPolicy) return true
if (!isAuthenticated) return false
return isGranted(item.requiredPolicy)
```

Use `requiresAuth` for menu items that only require login. Use `requiredPolicy` when the item should only be visible to users with a specific permission.

## Compound Policies

The template's `isPolicyGranted` helper supports simple compound expressions:

- `PermissionA || PermissionB`
- `PermissionA && PermissionB`

This is useful for menu entries that should be visible when the user has one of several related module permissions.

## Where Permissions Are Applied

The generated React app uses permissions in these places:

- **Users page**: the `/identity/users` route and sidebar entry require `AbpIdentity.Users`. The page links to the Admin Console for full user and role management.
- **Books page**: the route requires `MyProjectName.Books`; edit and delete actions check `MyProjectName.Books.Edit` and `MyProjectName.Books.Delete`.
- **Admin Console link**: the sidebar entry uses `requiresAuth` because the Admin Console performs its own module and route permission checks.

The Admin Console applies module-specific permissions for pages such as:

- Identity users and roles: `AbpIdentity.*`.
- OpenIddict applications and scopes: `OpenIddictPro.Application` and `OpenIddictPro.Scope`.
- Audit Logging UI: `AuditLogging.AuditLogs`.
- Text Template Management: `TextTemplateManagement.*`.
- AI Management: `AIManagement.*`.

## Multi-Tenancy

When a tenant is selected, the template stores the tenant ID in `sessionStorage` as `abp_tenant_id`. Permission and API requests send it with the `__tenant` header. This ensures the backend returns permissions and data for the selected tenant context.

## See Also

- [Authorization](./authorization.md)
- [HTTP Requests](./http-requests.md)
- [Authorization](../../../framework/fundamentals/authorization/index.md)
