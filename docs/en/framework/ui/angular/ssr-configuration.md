```json
//[doc-seo]
{
  "Description": "Learn how to configure Server-Side Rendering (SSR) for your Angular application in the ABP Framework to improve performance and SEO."
}
```

# SSR Configuration

[Server-Side Rendering (SSR)](https://angular.dev/guide/ssr) is a process that involves rendering pages on the server, resulting in initial HTML content that contains the page state. This allows the browser to show the page to the user immediately, before the JavaScript bundles are downloaded and executed.

SSR improves the **performance** (First Contentful Paint) and **SEO** (Search Engine Optimization) of your application.

## 1. Install ABP Angular SSR

The ABP Framework provides a schematic to easily add SSR support to your Angular application.

Run the following command in the root folder of your Angular application:

```shell
yarn ng generate @abp/ng.schematics:ssr-add
```

Alternatively, you can specify the project name if you have a multi-project workspace:

```shell
yarn ng generate @abp/ng.schematics:ssr-add --project MyProjectName
```

This command automates the setup process by installing necessary dependencies, creating server-side entry points, and updating your configuration files.

## 2. What Changes?

When you run the schematic, it performs the following actions:

### 2.1. Dependencies

It adds the following packages to your `package.json`:

-   **express**: A minimal and flexible Node.js web application framework.
-   **@types/express**: Type definitions for Express.
-   **openid-client**: A library for OpenID Connect (OIDC) relying party (RP) implementation, used for authentication on the server.

```json
{
  "dependencies": {
    "express": "^4.18.2",
    "openid-client": "^5.6.4"
  },
  "devDependencies": {
    "@types/express": "^4.17.17"
  }
}
```

**For Webpack projects only:**
-   **browser-sync** (Dev dependency): Used for live reloading during development.

### 2.2. Scripts & Configuration

The changes depend on the builder used in your project (Application Builder or Webpack).

#### Application Builder (esbuild)

If your project uses the **Application Builder** (`@angular/build:application`), the schematic:

-   **Scripts**: Adds `serve:ssr:project-name` to serve the SSR application.
-   **angular.json**: Updates the `build` target to enable SSR (`outputMode: 'server'`) and sets the SSR entry point.

```json
{
  "projects": {
    "MyProjectName": {
      "architect": {
        "build": {
          "options": {
            "outputPath": "dist/MyProjectName",
            "outputMode": "server",
            "ssr": {
              "entry": "src/server.ts"
            }
          }
        }
      }
    }
  }
}
```

-   **tsconfig**: Updates the application's `tsconfig` to include `server.ts`.

#### Webpack Builder

If your project uses the **Webpack Builder** (`@angular-devkit/build-angular:browser`), the schematic:

-   **Scripts**: Adds `dev:ssr`, `serve:ssr`, `build:ssr`, and `prerender` scripts.
-   **angular.json**: Adds new targets: `server`, `serve-ssr`, and `prerender`.
-   **tsconfig**: Updates the server's `tsconfig` to include `server.ts`.

### 2.3. Files

-   **server.ts**: This file is the main entry point for the server-side application.
    -   **Standalone Projects**: Generates a server entry point compatible with `bootstrapApplication`.
    -   **NgModule Projects**: Generates a server entry point compatible with `platformBrowserDynamic`.

```typescript
import {
    AngularNodeAppEngine,
    createNodeRequestHandler,
    isMainModule,
    writeResponseToNodeResponse,
} from '@angular/ssr/node';
import express from 'express';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { environment } from './environments/environment';
import { ServerCookieParser } from '@abp/ng.core';
import * as oidc from 'openid-client';

// ... (OIDC configuration and setup)

const app = express();
const angularApp = new AngularNodeAppEngine();

// ... (OIDC routes: /authorize, /logout, /)

/**
 * Serve static files from /browser
 */
app.use(
    express.static(browserDistFolder, {
        maxAge: '1y',
        index: false,
        redirect: false,
    }),
);

/**
 * Handle all other requests by rendering the Angular application.
 */
app.use((req, res, next) => {
    angularApp
        .handle(req)
        .then(response => {
            if (response) {
                res.cookie('ssr-init', 'true', {...secureCookie, httpOnly: false});
                return writeResponseToNodeResponse(response, res);
            } else {
                return next()
            }
        })
        .catch(next);
});

// ... (Start server logic)

export const reqHandler = createNodeRequestHandler(app);
```
-   **app.routes.server.ts**: Defines server-side routes and render modes (e.g., Prerender, Server, Client). This allows fine-grained control over how each route is rendered.

```typescript
import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
    {
        path: '**',
        renderMode: RenderMode.Server
    }
];
```

-   **app.config.server.ts**: Merges the application configuration with server-specific providers.

```typescript
import { mergeApplicationConfig, ApplicationConfig, provideAppInitializer, inject, PLATFORM_ID, TransferState } from '@angular/core';
import { isPlatformServer } from '@angular/common';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { appConfig } from './app.config';
import { serverRoutes } from './app.routes.server';
import { SSR_FLAG } from '@abp/ng.core';

const serverConfig: ApplicationConfig = {
    providers: [
        provideAppInitializer(() => {
            const platformId = inject(PLATFORM_ID);
            const transferState = inject<TransferState>(TransferState);
            if (isPlatformServer(platformId)) {
                transferState.set(SSR_FLAG, true);
            }
        }),
        provideServerRendering(withRoutes(serverRoutes)),
    ],
};

export const config = mergeApplicationConfig(appConfig, serverConfig);
```
-   **index.html**: Removes the loading spinner (`<div id="lp-page-loader"></div>`) to prevent hydration mismatches.

## 3. Running the Application

After the installation is complete, you can run your application with SSR support.

### Application Builder

To serve the application with SSR in development:

```shell
yarn start
# or
yarn ng serve
```

To serve the built application (production):

```shell
yarn run serve:ssr:project-name
```

### Webpack Builder

**Development:**

```shell
yarn run dev:ssr
```

**Production:**

```shell
yarn run build:ssr
yarn run serve:ssr
```

## 4. Authentication & SSR

The schematic installs `openid-client` to handle authentication on the server side. This ensures that when a user accesses a protected route, the server can validate their session or redirect them to the login page before rendering the content.

> Ensure your OpenID Connect configuration (in `environment.ts` or `app.config.ts`) is compatible with the server environment.

## 5. Render Modes & Hybrid Rendering

Angular 20 provides different rendering modes that you can configure per route in the `app.routes.server.ts` file to optimize performance and SEO.

### 5.1. Available Render Modes

```typescript
import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
    // Server-Side Rendering - renders on every request
    {
        path: 'dashboard',
        renderMode: RenderMode.Server
    },

    // Prerender (SSG) - renders at build time
    {
        path: 'about',
        renderMode: RenderMode.Prerender
    },

    // Client-Side Rendering - renders only in browser
    {
        path: 'admin/**',
        renderMode: RenderMode.Client
    },

    // Default fallback
    {
        path: '**',
        renderMode: RenderMode.Server
    }
];
```

#### RenderMode.Server (SSR)
Renders HTML on every request. Best for dynamic content, personalized pages, and pages requiring authentication.

#### RenderMode.Prerender (SSG)
Generates static HTML at build time. Best for marketing pages, blog posts, and content that doesn't change frequently.

For dynamic routes, use `getPrerenderParams`:

```typescript
{
    path: 'blog/:slug',
        renderMode: RenderMode.Prerender,
        getPrerenderParams: async () => {
        const posts = await fetchBlogPosts();
        return posts.map(post => ({ slug: post.slug }));
    }
}
```

#### RenderMode.Client (CSR)
Traditional client-side rendering. Best for highly interactive applications and admin panels that don't need SEO.

### 5.2. Hybrid Rendering

Combine different modes in one application for optimal results:

```typescript
export const serverRoutes: ServerRoute[] = [
    // Static pages
    { path: '', renderMode: RenderMode.Prerender },
    { path: 'about', renderMode: RenderMode.Prerender },

    // Dynamic pages
    { path: 'account', renderMode: RenderMode.Server },
    { path: 'orders', renderMode: RenderMode.Server },

    // Admin area
    { path: 'admin/**', renderMode: RenderMode.Client },
];
```

## 6. Hydration

Hydration is the process where Angular attaches to server-rendered HTML and makes it interactive. The ABP schematic automatically configures hydration for your application.

### 6.1. Common Hydration Issues

**Problem: Browser APIs on Server**

```typescript
// ❌ Bad - will fail on server
const width = window.innerWidth;

// ✅ Good - check platform
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID, inject } from '@angular/core';

export class MyComponent {
    platformId = inject(PLATFORM_ID);

    getWidth() {
        if (isPlatformBrowser(this.platformId)) {
            return window.innerWidth;
        }
        return 0;
    }
}
```

**Problem: Random or Time-Based Values**

```typescript
// ❌ Bad - generates different values on server and client
id = Math.random();
currentTime = new Date();

// ✅ Good - use TransferState for consistent data
import { TransferState, makeStateKey } from '@angular/core';

const TIME_KEY = makeStateKey<string>('time');

constructor(private transferState: TransferState) {
    if (isPlatformServer(this.platformId)) {
        this.transferState.set(TIME_KEY, new Date().toISOString());
    } else {
        this.time = this.transferState.get(TIME_KEY, new Date().toISOString());
    }
}
```

**Enable Debug Tracing:**

```typescript
// app.config.ts
import { provideClientHydration, withDebugTracing } from '@angular/platform-browser';

export const appConfig: ApplicationConfig = {
    providers: [
        provideClientHydration(withDebugTracing()),
    ]
};
```

## 7. Environment Variables

Configure your SSR application using environment variables in `server.ts`:

```typescript
// server.ts
const PORT = process.env['PORT'] || 4000;
const HOST = process.env['HOST'] || 'localhost';

// Start the server
if (isMainModule(import.meta.url)) {
    app.listen(PORT, () => {
        console.log(`Server running on http://${HOST}:${PORT}`);
    });
}
```

For production, set environment variables:

```bash
# .env file or environment configuration
NODE_ENV=production
PORT=4000
HOST=0.0.0.0
API_URL=https://api.yourdomain.com
```

## 8. Deployment

To deploy your Angular SSR application to a production server:

### 8.1. Build the Application

```shell
yarn build
# or if using Webpack builder
yarn run build:ssr
```

### 8.2. Prepare Artifacts

Copy the `dist/MyProjectName` folder to your server:

```
dist/MyProjectName/
├── browser/       # Client-side bundles
└── server/        # Server-side bundles (server.mjs)
```

### 8.3. Install Production Dependencies

On your server, install only the required dependencies (schematic already added them to package.json):

```shell
npm install --production
```

Required dependencies:
- `express`: Web server framework
- `openid-client`: Authentication support

### 8.4. Run the Server

**Development/Testing:**
```shell
node server/server.mjs
```

**Production (with PM2):**

Use [PM2](https://pm2.keymetrics.io/) to keep your application alive and manage restarts:

```shell
npm install -g pm2
pm2 start server/server.mjs --name "my-app"
pm2 startup  # Configure PM2 to start on boot
pm2 save     # Save current process list
```

## 9. Troubleshooting

### 9.1. "Window/Document is not defined"

Browser APIs don't exist on the server. Always check the platform:

```typescript
import { isPlatformBrowser } from '@angular/common';

if (isPlatformBrowser(this.platformId)) {
    // Safe to use window, document, localStorage, etc.
}
```

### 9.2. "LocalStorage is not defined"

ABP Core provides `AbpLocalStorageService` that implements the `Storage` interface and works safely on both server and client:

```typescript
import { AbpLocalStorageService } from '@abp/ng.core';

@Injectable({ providedIn: 'root' })
export class MyService {
    private storage = inject(AbpLocalStorageService);

    saveData(key: string, value: string): void {
        // Safe on both server and client
        this.storage.setItem(key, value);
    }

    getData(key: string): string | null {
        // Returns null on server, actual value on client
        return this.storage.getItem(key);
    }
}
```

`AbpLocalStorageService` implements all `Storage` methods:
- `getItem(key: string): string | null`
- `setItem(key: string, value: string): void`
- `removeItem(key: string): void`
- `clear(): void`
- `key(index: number): string | null`
- `length: number`

### 9.3. Hydration Mismatch Errors

If you see "NG0500" errors in the console:

1. Enable debug tracing (see section 6.1)
2. Check for dynamic content (dates, random IDs)
3. Ensure server and client render the same HTML
4. Use `TransferState` for data consistency

### 9.4. Avoiding Duplicate API Calls

ABP Core provides a `transferStateInterceptor` that automatically prevents duplicate HTTP GET requests during hydration. When you use `provideAbpCore()`, this interceptor is already active.

**How it works:**
- Server: Stores HTTP GET responses in `TransferState`
- Client: Reuses stored responses during hydration
- Automatically cleans up stored data after use

```typescript
// app.config.ts
import { provideAbpCore } from '@abp/ng.core';

export const appConfig: ApplicationConfig = {
    providers: [
        provideAbpCore(),
        // transferStateInterceptor is automatically included
    ]
};
```

The interceptor works with all HTTP GET requests made through `HttpClient`:

```typescript
// This service automatically benefits from the interceptor
@Injectable({ providedIn: 'root' })
export class UserService {
    private http = inject(HttpClient);

    getUsers() {
        // On server: Response is cached in TransferState
        // On client: Cached response is used (no duplicate request)
        return this.http.get<User[]>('/api/users');
    }
}
```

> [!NOTE]
> The interceptor only works with GET requests. POST, PUT, DELETE, and PATCH requests are not cached.

## Additional Resources

- [Angular SSR Official Guide](https://angular.dev/guide/ssr)
- [Angular Hydration Documentation](https://angular.dev/guide/hydration)
- [PM2 Process Manager](https://pm2.keymetrics.io/)

## Summary

The ABP Angular SSR schematic provides:
- ✅ Automatic SSR setup with necessary dependencies
- ✅ Server-side authentication with OpenID Connect
- ✅ Multiple render modes (Server, Prerender, Client, Hybrid)
- ✅ Hydration support for better performance

Configure render modes based on your needs, handle platform differences properly, and use environment variables for deployment configuration.
