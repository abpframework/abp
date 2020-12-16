# About Feature Libraries

ABP has an ever-growing number of feature modules and [introducing a new one](../../Module-Development-Basics.md) is always possible. When the UI is Angular, these features have modular Angular libraries accompanying them.

## Feature Library Content

Each library has at least two modules:

1. The main module contains all components, services, types, enums, etc. to deliver the required UI when the feature is loaded. From here on, we will refer to these modules as **"feature module"**.
2. There is also a **"config module"** per library which helps us configure applications to run these modules or make them accessible.

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

### 2. Import the Config Module

As of ABP v3.0, every lazy-loaded module has a config module available via a secondary entry point on the same package. Importing them in your root module looks like this:

```js
import { IdentityConfigModule } from "@abp/ng.identity/config";

@NgModule({
  imports: [
    // other imports
    IdentityConfigModule.forRoot(),
  ],
  // providers, declarations, and bootstrap
})
export class AppModule {}
```

We need the config modules for actions required before feature modules are loaded (lazily). For example, the above import configures the menu to display links to identity pages.

Furthermore, depending on the library, the `.forRoot` static method may receive some options that configure how the feature works.

### 3. Import the Feature Module

Finally, the feature module should be [loaded lazily via Angular router](https://angular.io/guide/lazy-loading-ngmodules). If you open the `/src/app/app-routing.module.ts` file, you should see `IdentityModule` is loaded exactly as follows:

```js
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
  // other routes
  {
    path: "identity",
    loadChildren: () =>
      import("@abp/ng.identity").then((m) => m.IdentityModule.forLazy()),
  },
  // other routes
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
```

When you load the identity feature like this, the "Users" page, for example, will have a route path of `/identity/users`. <sup id="a-modify-route">[1](#f-modify-route)</sup>

Depending on the library, the `.forLazy` static method may also receive some options that configure how the feature works.

---

<sup id="f-modify-route"><b>1</b></sup> _Libraries expect to work at a predefined path. Please check [how to patch a navigation element](./Modifying-the-Menu.md#how-to-patch-or-remove-a-navigation-element), if you want to use a different path from the default one (e.g. '/identity')._ <sup>[↩](#a-modify-route)</sup>
