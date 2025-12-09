```json
//[doc-seo]
{
    "Description": "Learn how to configure Server-Side Rendering (SSR) for your Angular application in the ABP Framework to improve performance and SEO."
}
```

# SSR Configuration

[Server-Side Rendering (SSR)](https://angular.io/guide/ssr) is a process that involves rendering pages on the server, resulting in initial HTML content that contains the page state. This allows the browser to show the page to the user immediately, before the JavaScript bundles are downloaded and executed.

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

## 5. Deployment

To deploy your Angular SSR application to a production server, follow these steps:

### 5.1. Build the Application

Run the build command to generate the production artifacts:

```shell
yarn build
# or if using Webpack builder
yarn run build:ssr
```

### 5.2. Prepare Artifacts

After the build is complete, you will find the output in the `dist` folder.
For the **Application Builder**, the output structure typically looks like this:

```
dist/MyProjectName/
├── browser/       # Client-side bundles
└── server/        # Server-side bundles and entry point (server.mjs)
```

You need to copy the entire `dist/MyProjectName` folder to your server.

### 5.3. Run the Server

On your server, navigate to the folder where you copied the artifacts and run the server using Node.js:

```shell
node server/server.mjs
```

> [!TIP]
> It is recommended to use a process manager like [PM2](https://pm2.keymetrics.io/) to keep your application alive and handle restarts.

```shell
pm2 start server/server.mjs --name "my-app"
```
