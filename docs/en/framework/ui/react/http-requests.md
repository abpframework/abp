```json
//[doc-seo]
{
    "Description": "Learn how HTTP requests are made in ABP React UI applications with Axios, runtime configuration, and ABP interceptors."
}
```

# HTTP Requests

ABP React UI templates use [Axios](https://axios-http.com/) for HTTP requests. The generated app contains a shared Axios instance with ABP-specific request and response interceptors, plus typed API modules for backend endpoints.

The shared client is defined in `src/lib/api/axios.ts` and exported as `api`.

## Base URL

The Axios base URL is resolved at request time from runtime configuration:

```ts
export function getApiBaseUrl(): string {
  const apiUrl = getApiUrl()
  if (apiUrl.startsWith('http://') || apiUrl.startsWith('https://')) {
    return apiUrl.replace(/\/$/, '') + '/api'
  }
  if (import.meta.env.DEV) {
    return '/api'
  }
  return apiUrl.replace(/\/$/, '') + '/api'
}
```

The API URL comes from:

1. `dynamic-env.json` -> `apis.default.url`
2. `VITE_API_URL`
3. `src/env.ts` generated fallback

In microservice solutions, `apis.default.url` normally points to the Web Gateway. In layered and single-layer solutions, it normally points to the HTTP API host.

## Shared Axios Instance

The template creates one shared instance:

```ts
export const api = axios.create({
  baseURL: '',
  headers: {
    'X-Requested-With': 'XMLHttpRequest',
    'Content-Type': 'application/json',
  },
})
```

Use this instance for application API modules instead of creating new Axios clients. It centralizes ABP headers, authentication, tenant handling, language handling, and redirects.

## Request Interceptor

Before each request, the template:

- Sets `baseURL` from runtime configuration.
- Adds `Authorization: Bearer <token>` from the OIDC user.
- Adds `__tenant` when the user has selected a tenant.
- Adds `Accept-Language` from i18next.
- Keeps default AJAX headers such as `X-Requested-With`.

```ts
api.interceptors.request.use(async (config) => {
  config.baseURL = getApiBaseUrl()

  const user = await userManager.getUser()
  if (user?.access_token) {
    config.headers.Authorization = `Bearer ${user.access_token}`
  }

  const tenantId = sessionStorage.getItem('abp_tenant_id')
  if (tenantId && !config.headers.__tenant) {
    config.headers.__tenant = tenantId
  }

  if (i18n?.language) {
    config.headers['Accept-Language'] =
      config.headers['Accept-Language'] ?? i18n.language
  }

  return config
})
```

## Response Interceptor

The response interceptor handles common authorization failures:

- `401 Unauthorized`: redirects to login unless `skipAuthRedirect` is set.
- `403 Forbidden`: redirects to `/403` unless `skip403Redirect` is set.
- Other errors are rejected so the caller can handle them.

```ts
api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const status = error.response?.status

    if (status === 401 && !error.config?.skipAuthRedirect) {
      await userManager.signinRedirect()
      return Promise.reject(new Error('Unauthorized - redirecting to login'))
    }

    if (status === 403 && !error.config?.skip403Redirect) {
      window.location.href = '/403'
      return Promise.reject(new Error('Forbidden'))
    }

    return Promise.reject(error)
  }
)
```

Use `skipAuthRedirect` or `skip403Redirect` for calls where the component should handle the error itself.

## Typed API Modules

The template organizes backend calls under `src/lib/api/`. For example, the Books sample defines DTOs and functions in `books.ts`:

```ts
import { api } from './axios'

export interface PagedResultDto<T> {
  items: T[]
  totalCount: number
}

export interface BookDto {
  id: string
  name?: string
  price: number
}

export async function getBooks(): Promise<PagedResultDto<BookDto>> {
  const { data } = await api.get<PagedResultDto<BookDto>>('/app/book', {
    params: {
      maxResultCount: 10,
      skipCount: 0,
    },
  })
  return data
}
```

Notice that the API module calls `/app/book`, not `/api/app/book`. The shared Axios base URL already includes the `/api` prefix when needed.

## Using Requests from Components

The template uses TanStack Query for server state:

```tsx
const { data, isLoading } = useQuery({
  queryKey: ['books', skipCount],
  queryFn: () =>
    getBooks({
      maxResultCount: 10,
      skipCount,
      sorting: 'creationTime desc',
    }),
})
```

Mutations use `useMutation` and invalidate related queries after success:

```tsx
const createMutation = useMutation({
  mutationFn: createBook,
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['books'] })
    toast.success(t('AbpUi::SavedSuccessfully'))
  },
})
```

## Adding a New API Module

Create a file under `src/lib/api/`:

```ts
import { api } from './axios'

export interface ProductDto {
  id: string
  name: string
}

export async function getProducts(): Promise<ProductDto[]> {
  const { data } = await api.get<ProductDto[]>('/app/product')
  return data
}
```

Then consume it from a component with TanStack Query:

```tsx
const productsQuery = useQuery({
  queryKey: ['products'],
  queryFn: getProducts,
})
```

## Development Proxy

In development, Vite proxies `/api`, `/connect`, and `/getEnvConfig`. This lets the React app use same-origin paths while calls are forwarded to the backend, Auth Server, or gateway configured by `VITE_API_URL` and `VITE_AUTH_URL`.

## See Also

- [Environment Variables](./environment-variables.md)
- [Authorization](./authorization.md)
- [Permission Management](./permission-management.md)
