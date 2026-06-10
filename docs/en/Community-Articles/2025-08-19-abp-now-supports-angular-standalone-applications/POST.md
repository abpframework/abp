# ABP Now Supports Angular Standalone Applications

We are excited to announce that **ABP now supports Angular’s standalone component structure** in the latest Studio update. This article walks you through how to generate a standalone application, outlines the migration steps, and highlights the benefits of this shift over traditional module-based architecture.

---

## Why Standalone?

Angular's standalone component architecture, which is introduced in version 14 and made default in version 19, is a major leap forward for Angular development. Here is why it matters:

### 🔧 Simplified Project Structure

Standalone components eliminate the need for `NgModule` wrappers. This leads to:

- Fewer files to manage
- Cleaner folder organization
- Reduced boilerplate

Navigating and understanding your codebase becomes easier for everyone on your team.

### 🚀 Faster Bootstrapping

Standalone apps simplify app initialization:

```ts
bootstrapApplication(AppComponent, appConfig);
```

This avoids the need for `AppModule` and speeds up startup times.

### 📦 Smaller Bundle Sizes

Since components declare their own dependencies, Angular can more effectively tree-shake unused code. Result? Smaller bundle sizes and faster load times.

### 🧪 Easier Testing & Reusability

Standalone components are self-contained. They declare their dependencies within the `imports` array, making them:

- Easier to test in isolation
- Easier to reuse in different contexts

### 🧠 Clearer Dependency Management

Standalone components explicitly define what they need. No more hidden dependencies buried in shared modules.

### 🔄 Gradual Adoption

You can mix and match standalone and module-based components. This allows for **incremental migration**, reducing risk in larger codebases. Here is the related document for the [standalone migration](https://angular.dev/reference/migrations/standalone).

---

## Getting Started: Creating a Standalone Angular App

Angular CLI makes it easy to start:

```bash
ng new my-app
```

With Angular 19, new apps follow this bootstrapping model:

```ts
// main.ts
import { bootstrapApplication } from "@angular/platform-browser";
import { appConfig } from "./app/app.config";
import { AppComponent } from "./app/app.component";

bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
```

The `app.config.ts` file replaces `AppModule`:

```ts
// app.config.ts
import { ApplicationConfig, provideZoneChangeDetection } from "@angular/core";
import { provideRouter } from "@angular/router";
import { routes } from "./app.routes";

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
  ],
};
```

Routing is defined in a simple `Routes` array:

```ts
// app.routes.ts
import { Routes } from "@angular/router";

export const routes: Routes = [];
```

---

## ABP Studio Support for Standalone Structure

Starting with the latest release (insert version number here), ABP Studio fully supports Angular's standalone structure. While the new format is encouraged, module-based structure will continue to be supported for backwards compatibility.

To try it out, simply update your ABP Studio to create apps with the latest version.

---

## What’s New in ABP Studio Templates?

When you generate an app using the latest ABP Studio, the project structure aligns with Angular's standalone architecture.

This migration is split into four parts:

1. **Package updates**
2. **Schematics updates**
3. **Suite code generation updates**
4. **Template refactors**

---

## Package Migration Details

Migration has been applied to packages in the [ABP GitHub repository](https://github.com/abpframework/abp/tree/dev/npm/ng-packs/packages). Here is an example from the Identity package.

### 🧩 Migrating Components

Components are made standalone, using:

```bash
ng g @angular/core:standalone
```

Example:

```ts
@Component({
  selector: 'abp-roles',
  templateUrl: './roles.component.html',
  providers: [...],
  imports: [
    ReactiveFormsModule,
    LocalizationPipe,
    ...
  ],
})
export class RolesComponent implements OnInit { ... }
```

### 🛣 Updating Routing

Old lazy-loaded routes using `forLazy()`:

```ts
{
  path: 'identity',
  loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy({...}))
}
```

Now replaced with:

```ts
{
  path: 'identity',
  loadChildren: () => import('@abp/ng.identity').then(c => c.createRoutes({...}))
}
```

### 🧱 Replacing Module Declarations

The old setup:

```ts
// identity.module.ts
@NgModule({
  imports: [IdentityRoutingModule, RolesComponent, UsersComponent],
})
export class IdentityModule {...}
```

```ts
//identity-routing.module
const routes: Routes = [...];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class IdentityRoutingModule {}
```

New setup:

```ts
// identity-routes.ts
export function provideIdentity(options: IdentityConfigOptions = {}): Provider[] {
  return [...];
}
export const createRoutes = (options: IdentityConfigOptions = {}): Routes => [
  {
    path: '',
    component: RouterOutletComponent,
    providers: provideIdentity(options),
    children: [
      {
        path: 'roles',
        component: ReplaceableRouteContainerComponent,
        data: {
          requiredPolicy: 'AbpIdentity.Roles',
          replaceableComponent: {
            key: eIdentityComponents.Roles,
            defaultComponent: RolesComponent,
          },
        },
        title: 'AbpIdentity::Roles',
      },
      ...
    ],
  },
];
```

---

## ABP Schematics Migration Details

You can reach details by checking [ABP Schematics codebase](https://github.com/abpframework/abp/tree/dev/npm/ng-packs/packages/schematics).

### 📚 Library creation

When you run the `abp create-lib` command, the prompter will ask you the `templateType`. It supports both module and standalone templates.

```ts
"templateType": {
  "type": "string",
  "description": "Type of the template",
  "enum": ["module", "standalone"],
  "x-prompt": {
    "message": "Select the type of template to generate:",
    "type": "list",
    "items": [
      { "value": "module", "label": "Module Template" },
      { "value": "standalone", "label": "Standalone Template" }
    ]
  }
},
```

---

## ABP Suite Code Generation Migration Details

ABP Suite will also be supporting both structures. If you have a project that is generated with the previous versions, the Suite will detect the structure in that way and generate the related code accordingly. Conversely, here is what is changed for the standalone migration:

**❌ Discarded module files**

```ts
// entity-one.module.ts
@NgModule({
  declarations: [],
  imports: [EntityOneComponent, EntityOneRoutingModule],
})
export class EntityOneModule {}
```

```ts
// entity-one-routing.module.ts
export const routes: Routes = [
  {
    path: "",
    component: EntityOneComponent,
    canActivate: [authGuard, permissionGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EntityOneRoutingModule {}
```

```ts
// app-routing.module.ts
{
  path: 'entity-ones',
  loadChildren: () =>
    import('./entity-ones/entity-one/entity-one.module').then(m => m.EntityOneModule),
},
```

**✅ Added routes configuration**

```ts
// entity-one.routes.ts
export const ENTITY_ONE_ROUTES: Routes = [
  {
    path: "",
    loadComponent: () => {
      return import("./components/entity-one.component").then(
        (c) => c.EntityOneComponent
      );
    },
    canActivate: [authGuard, permissionGuard],
  },
];
```

```ts
// app.routes.ts
{ path: 'entity-ones', children: ENTITY_ONE_ROUTES },
```

---

## Template Migration Details

### 🧭 Routing: `app.routes.ts`

```ts
// app.routes.ts
import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(m => m.HomeComponent),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.createRoutes()),
  },
  ...
];
```

### ⚙ Configuration: `app.config.ts`

```ts
// app.config.ts
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES),
    APP_ROUTE_PROVIDER,
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocale(),
        ...
      })
    ),
    provideAbpOAuth(),
    provideAbpThemeShared(),
    ...
  ],
};

```

### 🧼 Removed: `shared.module.ts`

This file has been removed to reduce unnecessary shared imports. Components now explicitly import what they need—leading to better encapsulation and less coupling.

---

## Common Problems

You may encounter these common problems that you would need to manage.

### 1. Missing Imports

In standalone structure, components must declare all their dependencies in `imports`. Forgetting this often causes template errors.

### 2. Mixed Structures

Combining modules and standalone in the same feature leads to confusion. Migrate features fully or keep them module-based.

### 3. Routing Errors

Incorrect migration from `forLazy()` to `createRoutes()` or `loadComponent` can break navigation. Double-check route configs.

### 4. Service Injection

Services provided in old modules may be missing. Add them in the component’s `providers` or `app.config.ts`.

### 5. Shared Module Habit

Reintroducing a shared module reduces the benefits of standalone. Import dependencies directly where needed.

---

## Conclusion

Angular’s standalone component architecture is a significant improvement for scalability, simplicity, and performance. With latest version of ABP Studio, you can adopt this modern approach with ease—without losing support for existing module-based projects.

**Ready to modernize your Angular development?**

Update your ABP Studio today and start building with standalone power!
