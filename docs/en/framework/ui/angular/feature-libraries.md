```json
//[doc-seo]
{
    "Description": "Learn how to implement and configure feature libraries in ABP Framework to enhance your Angular applications with modular functionality."
}
```

# About Feature Libraries

ABP has an ever-growing number of feature modules and [introducing a new one](../../architecture/modularity/basics.md) is always possible. When the UI is Angular, these features have modular Angular libraries accompanying them.

## Feature Library Content

Each library has at least two key elements:

1. A **feature definition** that encapsulates all components, services, types, enums, and routing logic needed to deliver the UI for a given feature. With standalone structure, this is often expressed through a `routes.ts` file and associated components, and we will refer to this as the **"feature structure"**.
2. A **configuration provider** that exposes setup logic, such as `provideMyProjectNameConfig()` functions or environment, specific tokens—allowing the feature to be initialized or integrated differently across applications. We will refer to this as the **configuration structure**.

## How to Add a Feature Library to Your Project

<!-- TODO: Insert info on CLI `add-module` command here when the schematic is ready. -->

The manual setup of a feature library has three steps:

### 1. Install the Library

Feature libraries are usually published as an npm package. If a library you want to use does not exist in your project, you may install it via the following command:

```shell
yarn add @my-company-name/my-project-name
```

...or...

```shell
npm install @my-company-name/my-project-name
```

The `my-company-name` and `my-project-name` parts are going to change according to the package you want to use. For example, if we want to install the ABP Identity module, the package installation will be as seen below:

```shell
yarn add @abp/ng.identity
```

> Identity is used just as an example. If you have initiated your project with ABP CLI or ABP Suite, the identity library will already be installed and configured in your project.

### 2. Import the Configuration Provider

As of ABP v9.3, every lazy-loaded route has a config provider available via a secondary entry point on the same package. Importing them in your root configuration looks like this:

```ts
import { provideIdentityConfig } from "@abp/ng.identity/config";

export const appConfig: ApplicationConfig = {
  providers: [
    // other providers
    provideIdentityConfig(),
  ],
};
```

We need the config providers for actions required before feature structure is loaded (lazily). For example, the above import configures the menu to display links to identity pages.

Furthermore, depending on the library, the `.createRoutes` static method may receive some options that configure how the feature works.

### 3. Import the Feature Definition

Finally, the feature structure should be [loaded lazily via Angular router](https://angular.dev/reference/migrations/route-lazy-loading). In a standalone setup, routing is typically defined in a `app.routes.ts` file, and feature modules are replaced with route-level feature definitions. You should see the identity routes configured like this:

```js
import { Routes } from "@angular/router";

const APP_ROUTES: Routes = [
  // other routes
  {
    path: "identity",
    loadChildren: () =>
      import("@abp/ng.identity").then((m) => m.createRoutes()),
  },
  // other routes
];
```

When you load the identity feature like this, the "Users" page, for example, will have a route path of `/identity/users`. <sup id="a-modify-route">[1](#f-modify-route)</sup>

Depending on the library, the `.createRoutes` static method may also receive some options that configure how the feature works.

---

<sup id="f-modify-route"><b>1</b></sup> _Libraries expect to work at a predefined path. Please check [how to patch a navigation element](./modifying-the-menu.md#how-to-patch-or-remove-a-navigation-element), if you want to use a different path from the default one (e.g. '/identity')._ <sup>[↩](#a-modify-route)</sup>
