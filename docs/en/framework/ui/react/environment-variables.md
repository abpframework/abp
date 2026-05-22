```json
//[doc-seo]
{
    "Description": "Learn how runtime configuration and environment variables work in ABP React UI applications."
}
```

# Environment Variables

ABP React UI applications use a runtime configuration file and Vite environment variables together. The template is preconfigured by ABP Studio's modern wizard, available with ABP Studio **v3.0+**, so a newly created solution already contains working local values for the API, Auth Server, OpenIddict client, and Admin Console link.

You usually change these values when moving the application to another environment such as staging or production.

## Configuration Sources

The React template reads configuration from these places:

- `dynamic-env.json`: runtime configuration that can be changed without rebuilding the application.
- `public/dynamic-env.json`: the file served by the app. The Vite build copies the root `dynamic-env.json` into this location when it exists.
- `src/env.ts`: local fallback values used when runtime configuration is not loaded.
- `.env` files / shell variables: Vite variables such as `VITE_API_URL`, `VITE_AUTH_URL`, and `VITE_APP_URL`.

For layered and single-layer modern templates, the React app is in the `react/` folder. For the microservice modern template, it is in `apps/react/`.

## `dynamic-env.json`

The runtime configuration file has the same purpose as Angular's dynamic environment configuration: it lets you deploy the same build artifact to different environments and change the API or authentication endpoints at runtime.

```json
{
  "application": {
    "baseUrl": "https://localhost:3000",
    "name": "Acme.BookStore"
  },
  "oAuthConfig": {
    "issuer": "https://localhost:44301/",
    "redirectUri": "https://localhost:3000",
    "clientId": "Acme_BookStore_App",
    "scope": "offline_access openid profile email phone AuthServer IdentityService AdministrationService"
  },
  "apis": {
    "default": {
      "url": "https://localhost:44300",
      "rootNamespace": "Acme.BookStore"
    }
  },
  "adminConsoleUrl": "https://localhost:44307"
}
```

The template loads `/dynamic-env.json` first and then tries `/getEnvConfig` for compatibility with environments that expose the file through that endpoint.

## Available Values

| Key | Description |
| --- | --- |
| `application.baseUrl` | Public URL of the React application. It is used as a fallback for OAuth redirect URLs. |
| `application.name` | Application name. |
| `application.logoUrl` | Optional logo URL for application branding. |
| `oAuthConfig.issuer` | Auth Server / OpenIddict authority URL. |
| `oAuthConfig.redirectUri` | Redirect URI registered for the React OpenIddict client. |
| `oAuthConfig.clientId` | OpenIddict client ID. The main React app uses `<ProjectName>_App`. |
| `oAuthConfig.scope` | OAuth scopes requested by the SPA. |
| `apis.default.url` | Backend API base URL. In microservice solutions, this normally points to the Web Gateway. |
| `apis.default.rootNamespace` | Root namespace used by generated API code and module-specific clients. |
| `adminConsoleUrl` | Origin of the Admin Console app. The React template uses it to open `/admin-console`. |

The `DynamicEnv` type also includes fields such as `production`, `oAuthConfig.requireHttps`, `oAuthConfig.responseType`, `oAuthConfig.strictDiscoveryDocumentValidation`, and `oAuthConfig.skipIssuerCheck`. The template's OIDC setup always uses the Authorization Code flow by setting `responseType` to `code`.

## Vite Variables

The React template uses Vite and reads environment variables with `loadEnv(mode, process.cwd(), '')`, so variables are not limited to the `VITE_` prefix inside `vite.config.ts`.

The important variables for developers are:

| Variable | Description |
| --- | --- |
| `VITE_API_URL` | Overrides the backend API or gateway URL used by the dev proxy and runtime fallback. |
| `VITE_AUTH_URL` | Overrides the Auth Server URL used by the dev proxy and runtime fallback. If omitted, the dev proxy can fall back to `VITE_API_URL`. |
| `VITE_APP_URL` | Overrides the React app URL used as the OAuth redirect URI fallback. |

Example:

```bash
VITE_API_URL=https://api.bookstore.example.com
VITE_AUTH_URL=https://auth.bookstore.example.com
VITE_APP_URL=https://bookstore.example.com
```

## What ABP Studio Preconfigures

When a React solution is created with ABP Studio v3.0+ or `abp new --modern`, the template fills these values from the generated solution configuration:

- Local launch ports for the React app, Web Gateway/API host, Auth Server, and Admin Console.
- The OpenIddict client ID, usually `<ProjectName>_App`.
- OAuth scopes based on the selected modules, such as Identity, Administration, SaaS, Audit Logging, GDPR, File Management, AI Management, Language Management, or Chat.
- `adminConsoleUrl` when the template includes a separate Admin Console application.

For local development, these generated values should work without manual changes. For production, update the API URL, Auth Server URL, redirect URI, client ID if you changed the seeded client, and any environment-specific scopes.

## Development Proxy

In development, `vite.config.ts` proxies these paths:

- `/api` to `VITE_API_URL` or the generated API/gateway URL.
- `/connect` to `VITE_AUTH_URL`, `VITE_API_URL`, or the generated Auth Server URL.
- `/getEnvConfig` to `VITE_API_URL` or the generated API/gateway URL.

This allows the React app to call same-origin paths during development while the backend services run on their own ports.

## Deployment

For deployment, prefer changing `dynamic-env.json` instead of rebuilding the React application for each environment. The file should be served with `application/json` content type and should not be rewritten to `index.html` by SPA fallback rules.

If your server exposes `/getEnvConfig`, configure it to return the same JSON content as `dynamic-env.json`.

## See Also

- [React UI](./index.md)
- [Authorization](./authorization.md)
- [HTTP Requests](./http-requests.md)
