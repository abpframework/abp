```json
//[doc-seo]
{
    "Description": "Learn how to build modern web applications with ABP using React UI — a React-first approach with a dedicated Admin Console, built on Vite, shadcn/ui, Zod, and Axios."
}
```

# React UI

## Introduction

ABP provides a **React UI** option for building modern, client-side web applications. The React UI is part of the **modern template system** and is only available when creating a solution with the `--modern` flag via the [ABP CLI](../../../cli/index.md) or through the **Modern Wizard** in [ABP Studio](../../../studio/index.md).

> React UI is **not** available in the legacy (non-modern) templates. You must use the modern template system to get a React-based solution. See [Creating a Solution](#creating-a-solution) below.

The React UI is built on a modern, industry-standard stack:

| Technology | Purpose |
|---|---|
| [Vite](https://vitejs.dev/) | Build tool and dev server |
| [React](https://react.dev/) | UI framework |
| [shadcn/ui](https://ui.shadcn.com/) | Component library (built on Radix UI + Tailwind CSS) |
| [Zod](https://zod.dev/) | Schema validation |
| [Axios](https://axios-http.com/) | HTTP client |
| [Vitest](https://vitest.dev/) | Unit testing |
| [React Router](https://reactrouter.com/) | Client-side routing |
| [OpenID Connect / OIDC](https://openid.net/connect/) | Authentication (via the ABP Auth Server) |

## React App and Admin Console

Every modern React solution consists of two parts: **your React application** and the **ABP Admin Console**.

### React App (Your Application)

This is **your application** — the user-facing SPA that you own and customize freely. It comes with:

- A sample **Books CRUD page** (when `--sample-crud-page` is used) demonstrating how to build a full create/read/update/delete page with the ABP backend
- A plain **Users page** as a minimal reference
- Pre-configured authentication via OIDC against the ABP Auth Server
- Pre-configured HTTP client (Axios) with ABP API integration

This is where you build your business-specific pages and features.

The location of the React app differs by template type:

- **Layered (`app --modern`) and Single-layer (`app-nolayers --modern`)**: the React app lives at the **root of the solution** (alongside the backend projects).
- **Microservice (`microservice --modern`)**: the React app lives at `apps/react/`.

### React Admin Console (`Volo.Abp.AdminConsole`)

The **ABP Admin Console** is a pre-built React application that provides all standard ABP module management pages. It is delivered via the `Volo.Abp.AdminConsole` NuGet package and requires no modification on your part — it is updated automatically when you update your ABP packages.

The Admin Console includes pages for:

- Users & Roles management
- Organization Units
- Settings
- Audit Logs
- OpenIddict (Application, Scope management)
- Language Management *(if included)*
- Text Template Management *(if included)*
- GDPR *(if included)*
- SaaS / Tenant Management *(if included)*
- And all other optional module pages based on your solution configuration

How the Admin Console is hosted also differs by template type:

- **Layered and Single-layer templates**: The `Volo.Abp.AdminConsole` package is added directly to your `*.HttpApi.Host` project. It serves the Admin Console React app at the `/admin-console/*` path of your backend (e.g., `https://localhost:44300/admin-console/`). There is no separate `apps/react-admin-console/` folder — the Admin Console UI is embedded in and served by the backend.
- **Microservice template**: The Admin Console runs as a standalone React app at `apps/react-admin-console/`, served through the Web Gateway (YARP) alongside the main React app.

In both cases the Admin Console is accessible from the main React app via a navigation link.

## Creating a Solution

### Using ABP CLI

Pass the `--modern` flag to `abp new`:

````bash
# Layered app with React UI (default when --modern is used)
abp new Acme.BookStore --template app --modern

# Single-layer app with React UI
abp new Acme.BookStore --template app-nolayers --modern

# Microservice solution with React UI + React Admin Console
abp new Acme.BookStore --template microservice --modern
````

The `react` UI framework is the default when `--modern` is specified. You can also pass it explicitly:

````bash
abp new Acme.BookStore --template app --modern --ui-framework react
````

To create a solution without any UI (API-only backend):

````bash
abp new Acme.BookStore --template app --modern --ui-framework no-ui
````

See the [ABP CLI documentation](../../../cli/index.md#modern-templates) for the full list of options.

### Using ABP Studio

Open ABP Studio and use the **New Solution** wizard. Select the **Modern** template variant and choose **React** as the UI framework. The wizard guides you through all available options and generates the solution.

## Solution Structure

The layout of the React-related files depends on the template type.

### Layered and Single-layer Templates

For `app --modern` and `app-nolayers --modern`, the React app lives at the root of the solution alongside the backend projects. The Admin Console is embedded in the backend via the `Volo.Abp.AdminConsole` NuGet package — there is no separate React Admin Console folder.

```
Acme.BookStore/
├── react/                              # Your React application
│   ├── src/
│   │   ├── pages/                      # Your page components
│   │   │   ├── books/                  # Sample Books CRUD page
│   │   │   └── users/                  # Sample Users page
│   │   ├── components/                 # Shared UI components
│   │   ├── lib/                        # Utilities, API clients
│   │   ├── hooks/                      # Custom React hooks
│   │   └── main.tsx                    # Entry point
│   ├── public/
│   │   └── dynamic-env.json            # Runtime configuration
│   ├── package.json
│   └── vite.config.ts
├── src/
│   ├── Acme.BookStore.Application/
│   ├── Acme.BookStore.Domain/
│   ├── Acme.BookStore.EntityFrameworkCore/
│   └── Acme.BookStore.HttpApi.Host/    # Hosts Admin Console at /admin-console/*
└── ...
```

### Microservice Template

For `microservice --modern`, both the React app and the React Admin Console are standalone apps under the `apps/` directory, each served through the Web Gateway (YARP).

```
Acme.BookStore/
├── apps/
│   ├── react/                          # Your React application
│   │   ├── src/
│   │   │   ├── pages/
│   │   │   ├── components/
│   │   │   ├── lib/
│   │   │   ├── hooks/
│   │   │   └── main.tsx
│   │   ├── public/
│   │   │   └── dynamic-env.json        # Runtime configuration
│   │   ├── package.json
│   │   └── vite.config.ts
│   ├── react-admin-console/            # ABP Admin Console (managed by ABP)
│   │   ├── public/
│   │   │   └── dynamic-env.json
│   │   └── ...
│   └── auth-server/                    # OpenIddict Auth Server
├── gateways/
│   └── web/                            # YARP reverse proxy for React apps
├── services/
│   ├── identity/
│   ├── administration/
│   └── ...
└── ...
```

## Configuration

### Runtime Configuration (`dynamic-env.json`)

The React app reads its runtime configuration from `public/dynamic-env.json`. This file is loaded at startup and allows you to change settings without rebuilding the application — useful for different environments (development, staging, production).

```json
{
  "oAuthConfig": {
    "issuer": "https://localhost:44301/",
    "clientId": "Acme.BookStore_App",
    "scope": "openid profile email offline_access BookStore"
  },
  "apis": {
    "default": {
      "url": "https://localhost:44300",
      "rootNamespace": "Acme.BookStore"
    }
  }
}
```

| Key | Description |
|---|---|
| `oAuthConfig.issuer` | URL of the OpenIddict Auth Server |
| `oAuthConfig.clientId` | The OpenIddict client ID registered for this app |
| `oAuthConfig.scope` | OAuth scopes to request |
| `apis.default.url` | Base URL of the backend API |
| `apis.default.rootNamespace` | Root namespace used for API proxy generation |

For the **microservice** modern template, the `apis.default.url` points to the **Web Gateway** (YARP reverse proxy) instead of a single backend host.

### Admin Console Configuration

For the **microservice** template, the Admin Console has its own `dynamic-env.json` (at `apps/react-admin-console/public/dynamic-env.json`) and uses a separate OpenIddict client (`<ProjectName>_AdminConsole`).

For **layered and single-layer** templates, the Admin Console is embedded in the backend via the `Volo.Abp.AdminConsole` package and does not have a separate configuration file — it inherits its settings from the backend host.

## Authentication

Both the React app and the Admin Console authenticate using **OpenID Connect (OIDC)** against the ABP Auth Server (OpenIddict). The auth flow is handled transparently — when a user visits a protected page, they are redirected to the Auth Server login page and returned to the app after successful authentication. All clients use the **Authorization Code flow with PKCE**, which is the recommended flow for SPAs.

The number of seeded OpenIddict clients depends on the template:

- **Layered and Single-layer templates**: one client is seeded — `<ProjectName>_App` for the React SPA. The Admin Console is embedded in the backend and shares the same authentication context.
- **Microservice template**: two clients are seeded — `<ProjectName>_App` for the main React SPA and `<ProjectName>_AdminConsole` for the standalone React Admin Console app.

## Making API Calls

The React app uses **Axios** as the HTTP client, pre-configured with:

- The base URL from `dynamic-env.json`
- Automatic Bearer token injection from the OIDC session
- ABP-compatible error handling (reads `error.data` from ABP error responses)

**Example: Fetching a list of books**

```typescript
import { useQuery } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

interface BookDto {
  id: string;
  name: string;
  type: number;
  publishDate: string;
  price: number;
}

interface PagedResult<T> {
  items: T[];
  totalCount: number;
}

export function useBooks() {
  return useQuery({
    queryKey: ['books'],
    queryFn: () =>
      apiClient
        .get<PagedResult<BookDto>>('/api/app/book')
        .then((res) => res.data),
  });
}
```

## Localization

The React app integrates with ABP's [localization system](../../../framework/fundamentals/localization.md). Localization resources are fetched from the backend at startup via the `/api/abp/application-configuration` endpoint and made available throughout the app.

**Example: Using localization in a component**

```typescript
import { useLocalization } from '@/hooks/use-localization';

export function MyComponent() {
  const { l } = useLocalization('BookStore');

  return <h1>{l('Books')}</h1>;
}
```

The `l(key)` function looks up the key in the loaded localization resources for the given resource name (`'BookStore'` in this example).

## Authorization

Permission checks are available via a hook that reads the current user's granted permissions from the ABP application configuration:

```typescript
import { usePermissions } from '@/hooks/use-permissions';

export function BooksPage() {
  const { isGranted } = usePermissions();

  return (
    <div>
      {isGranted('BookStore.Books.Create') && (
        <button>Create Book</button>
      )}
    </div>
  );
}
```

Permissions are defined on the server side using ABP's [permission system](../../../framework/fundamentals/authorization/index.md) and are automatically available in the React app.

## Tech Stack Details

### shadcn/ui Components

The React app uses [shadcn/ui](https://ui.shadcn.com/) as the component library. shadcn/ui is not a traditional npm package — components are copied directly into your project under `src/components/ui/`, giving you full ownership and the ability to customize them freely.

Common components available out of the box include: `Button`, `Input`, `Table`, `Dialog`, `Form`, `Select`, `Tabs`, `Card`, `Badge`, `Dropdown Menu`, and more.

### Form Validation with Zod

Forms use [Zod](https://zod.dev/) schemas for validation, integrated with [React Hook Form](https://react-hook-form.com/):

```typescript
import { z } from 'zod';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';

const createBookSchema = z.object({
  name: z.string().min(1).max(128),
  price: z.number().min(0),
  publishDate: z.string(),
});

type CreateBookInput = z.infer<typeof createBookSchema>;

export function CreateBookForm() {
  const form = useForm<CreateBookInput>({
    resolver: zodResolver(createBookSchema),
  });

  // ...
}
```

### Testing with Vitest

The React app is pre-configured with [Vitest](https://vitest.dev/) for unit testing:

```bash
# Run tests
npm run test

# Run tests with coverage
npm run test:coverage
```

## Running the Application

### Development

1. Start the backend (from ABP Studio or `dotnet run` in the `*.HttpApi.Host` project).
2. Navigate to the React app directory and start the dev server.

For **layered and single-layer** templates, the React app is at the solution root:

````bash
cd react
npm install
npm run dev
````

For the **microservice** template, it is under `apps/`:

````bash
cd apps/react
npm install
npm run dev
````

The app will be available at `https://localhost:3000` (or the port configured in `vite.config.ts`).

3. Access the Admin Console:
   - **Layered / Single-layer**: navigate to the backend URL + `/admin-console/` (e.g., `https://localhost:44300/admin-console/`).
   - **Microservice**: navigate to the Web Gateway URL + `/admin-console/`.

### Production Build

````bash
# Layered / Single-layer
cd react && npm run build

# Microservice
cd apps/react && npm run build
````

The output is placed in `dist/` and can be served by any static file host or CDN.

## Accessing the Admin Console

The Admin Console is accessible from within the main React app. After logging in, you will find a link to the Admin Console in the navigation. Clicking it opens the Admin Console, which is served by the backend at `/admin-console/*`.

The Admin Console provides full management capabilities for:

- **Identity**: Users, Roles, Claims, Organization Units
- **OpenIddict**: Applications, Scopes
- **Settings**: Application-wide and tenant-level settings
- **Audit Logs**: View and filter audit log entries
- **Language Management** *(optional)*
- **Text Template Management** *(optional)*
- **GDPR** *(optional)*
- **SaaS / Tenant Management** *(optional)*
- **And more**, depending on the modules included in your solution

## Customization

### Adding New Pages

Create a new file under `src/pages/` and register the route in the router configuration:

```typescript
// src/router.tsx (or similar)
import { BooksPage } from './pages/books/books-page';
import { MyNewPage } from './pages/my-feature/my-new-page';

const routes = [
  { path: '/books', element: <BooksPage /> },
  { path: '/my-feature', element: <MyNewPage /> },
  // ...
];
```

### Adding Menu Items

Add entries to the navigation configuration to include your new pages in the sidebar or top navigation:

```typescript
// src/config/navigation.ts (or similar)
export const navigationItems = [
  { label: 'Books', path: '/books', icon: BookIcon },
  { label: 'My Feature', path: '/my-feature', icon: StarIcon },
];
```

### Customizing shadcn/ui Components

Since shadcn/ui components live in `src/components/ui/`, you can modify them directly. For example, to change the default button variant or add a new variant, edit `src/components/ui/button.tsx`.

## See Also

- [ABP CLI — Modern Templates](../../../cli/index.md#modern-templates)
- [ABP Studio](../../../studio/index.md)
- [Localization](../../../framework/fundamentals/localization.md)
- [Authorization / Permissions](../../../framework/fundamentals/authorization/index.md)
- [Auto API Controllers](../../../framework/api-development/auto-controllers.md)
