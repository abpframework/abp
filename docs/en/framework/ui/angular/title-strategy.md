```json
//[doc-seo]
{
  "Description": "Configure localized Angular route titles, the application-name suffix, and a custom TitleStrategy in an ABP application."
}
```

# Document Title Strategy

`provideAbpCore` registers `AbpTitleStrategy` as Angular's default `TitleStrategy`. It reads the deepest active route title, localizes it, and updates the browser document title.

## Setting a Route Title

Use Angular's `title` route property. The value can be an ABP localization key:

```ts
import { Routes } from '@angular/router';
import { BooksComponent } from './books.component';

export const routes: Routes = [
  {
    path: 'books',
    component: BooksComponent,
    title: 'BookStore::Menu:Books',
  },
];
```

With an application name of `Book Store`, the resulting title is:

```text
Books | Book Store
```

The strategy resolves the application name from the `::AppName` localization key. If a route has no title, it uses only the application name. It also recalculates the active title when the language changes.

## Removing the Application-Name Suffix

Provide `DISABLE_PROJECT_NAME` with `true` to omit the application name from routes that have a title:

```ts
import { DISABLE_PROJECT_NAME } from '@abp/ng.core';
import { ApplicationConfig } from '@angular/core';

export const appConfig: ApplicationConfig = {
  providers: [
    {
      provide: DISABLE_PROJECT_NAME,
      useValue: true,
    },
  ],
};
```

The route above then produces `Books`. A route without a title still falls back to the application name.

## Replacing the Strategy

Create an Angular `TitleStrategy` and pass it to the `withTitleStrategy` feature when the application needs a different title convention:

```ts
import { Title } from '@angular/platform-browser';
import { RouterStateSnapshot, TitleStrategy } from '@angular/router';
import { inject, Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class CustomTitleStrategy extends TitleStrategy {
  private readonly title = inject(Title);

  override updateTitle(routerState: RouterStateSnapshot): void {
    const routeTitle = this.buildTitle(routerState);
    this.title.setTitle(routeTitle ? `My Application - ${routeTitle}` : 'My Application');
  }
}
```

Register it together with the normal ABP Core options:

```ts
import { provideAbpCore, withOptions, withTitleStrategy } from '@abp/ng.core';
import { registerLocaleForEsBuild } from '@abp/ng.core/locale';
import { ApplicationConfig } from '@angular/core';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAbpCore(
      withOptions({
        environment,
        registerLocaleFn: registerLocaleForEsBuild(),
      }),
      withTitleStrategy(CustomTitleStrategy),
    ),
  ],
};
```
