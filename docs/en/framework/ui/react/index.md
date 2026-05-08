```json
//[doc-seo]
{
    "Description": "Learn how to build modern web applications with ABP React UI, including runtime configuration, authentication, Admin Console, shadcn/ui components, and testing."
}
```

# React UI

ABP provides a **React UI** option for building modern, client-side web applications. React UI is part of the **modern template system** and is available with **ABP Studio v3.0+** through the Modern Wizard or with `abp new --modern` using [ABP CLI](../../../cli/index.md).

React UI is not available in classic, non-modern templates. Use ABP Studio's modern template flow or `Volo.Abp.Studio.Cli` to create a React-based solution.

## Technology Stack

The React UI template is built with:

| Technology | Purpose |
| --- | --- |
| [Vite](https://vite.dev/) | Build tool and dev server |
| [React](https://react.dev/) | UI framework |
| [TanStack Router](https://tanstack.com/router) | Client-side routing |
| [TanStack Query](https://tanstack.com/query) | Server state and API request orchestration |
| [shadcn/ui](https://ui.shadcn.com/) | Source-owned component library built on Radix UI and Tailwind CSS |
| [Zod](https://zod.dev/) | Schema validation |
| [React Hook Form](https://react-hook-form.com/) | Form state management |
| [Axios](https://axios-http.com/) | HTTP client |
| [Vitest](https://vitest.dev/) | Unit testing |
| [OpenID Connect / OIDC](https://openid.net/connect/) | Authentication against the ABP Auth Server |

The template also includes ABP-specific NPM packages:

- [`@volo/abp-app-config`](https://github.com/volosoft/volo/tree/dev/abp/npm/packs/abp-app-config)
- [`@volo/abp-oidc-auth`](https://github.com/volosoft/volo/tree/dev/abp/npm/packs/abp-oidc-auth)
- [`@volo/abp-react-app-config`](https://github.com/volosoft/volo/tree/dev/abp/npm/packs/abp-react-app-config)
- [`@volo/abp-react-oidc-auth`](https://github.com/volosoft/volo/tree/dev/abp/npm/packs/abp-react-oidc-auth)

## React App and Admin Console

A modern React solution contains two UI surfaces:

- **Your React application**: the developer-owned SPA where you build application-specific pages and features.
- **ABP Admin Console**: the React-based administration UI for ABP modules.

The Admin Console is provided by the `Volo.Abp.AdminConsole` NuGet package in layered and single-layer templates. In microservice templates, it is also generated as a separate `apps/react-admin-console/` app and served through the Web Gateway.

See [Admin Console](./admin-console.md) for hosting, module discovery, and permission details.

## Solution Structure

The React app location depends on the modern template type:

- **Layered (`app --modern`) and single-layer (`app-nolayers --modern`)**: the React app lives in the `react/` folder at the solution root.
- **Microservice (`microservice --modern`)**: the React app lives at `apps/react/`.

Typical structure:

```text
react/
├── dynamic-env.json
├── public/
├── src/
│   ├── components/
│   ├── lib/
│   ├── locales/
│   ├── pages/
│   ├── routes/
│   └── main.tsx
├── package.json
├── vite.config.ts
└── vitest.config.ts
```

## Creating a Solution

Install or update `Volo.Abp.Studio.Cli`, then create a modern solution:

```bash
# Layered app with React UI
abp new Acme.BookStore --template app --modern --ui-framework react

# Single-layer app with React UI
abp new Acme.BookStore --template app-nolayers --modern --ui-framework react

# Microservice solution with React UI
abp new Acme.BookStore --template microservice --modern --ui-framework react
```

You can also use ABP Studio v3.0+ and select the modern template flow in the New Solution wizard. The wizard preconfigures local ports, runtime configuration, OIDC clients, theme options, and React/Admin Console wiring based on the selected template and modules.

## Running the Application

Start the backend from ABP Studio or by running the backend host projects, then start the React development server.

For layered and single-layer templates:

```bash
cd react
npm install
npm run dev
```

For microservice templates:

```bash
cd apps/react
npm install
npm run dev
```

Run tests with:

```bash
npm run test
```

Build for production with:

```bash
npm run build
```

## Documentation Map

Use these pages to learn each part of the React UI:

- [Environment Variables](./environment-variables.md): runtime configuration, `dynamic-env.json`, Vite variables, and Studio-generated defaults.
- [Authorization](./authorization.md): OIDC, Authorization Code flow with PKCE, auth provider, hooks, and route guards.
- [Localization](./localization.md): i18next, local JSON resources, ABP localization keys, and request culture.
- [Permission Management](./permission-management.md): fetching granted policies, `usePermissions()`, route protection, and conditional UI.
- [HTTP Requests](./http-requests.md): Axios setup, interceptors, typed API modules, and TanStack Query usage.
- [Customization](./customization.md): changing pages, themes, sidebar items, user menu entries, and shadcn/ui components.
- [Components](./components/index.md): component architecture, UI primitives, layout components, forms, and routing.
- [Unit Testing](./unit-testing.md): Vitest, React Testing Library, examples, and test workflow.
- [Admin Console](./admin-console.md): the `Volo.Abp.AdminConsole` package, `/admin-console/*` hosting, module discovery, and optional modules.

## See Also

- [ABP Studio](../../../studio/index.md)
- [ABP CLI](../../../cli/index.md)
- [Authorization](../../../framework/fundamentals/authorization/index.md)
- [Localization](../../../framework/fundamentals/localization.md)
