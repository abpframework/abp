```json
//[doc-seo]
{
    "Description": "Learn how to simplify router event handling in Angular with `RouterEvents`, making your code cleaner and more efficient."
}
```

# Router Events Simplified

`RouterEvents` is a utility service for filtering specific router events and reacting to them. Please see [this page in Angular docs](https://angular.io/api/router/Event) for available router events.

## Benefit

You can use router events directly and filter them as seen below:

```js
import {
  NavigationEnd,
  NavigationError,
  NavigationCancel,
  Router,
} from '@angular/router';
import { filter } from 'rxjs/operators';
import { inject, Injectable } from '@angular/core';

@Injectable()
class SomeService {
  private router = inject(Router);

  navigationFinish$ = this.router.events.pipe(
    filter(
      event =>
        event instanceof NavigationEnd ||
        event instanceof NavigationError ||
        event instanceof NavigationCancel,
    ),
  );
  /* Observable<Event> */
}
```

However, `RouterEvents` makes filtering router events easier.

```js
import { RouterEvents } from '@abp/ng.core';

@Injectable()
class SomeService {
  private routerEvents = inject(RouterEvents);

  navigationFinish$ = this.routerEvents.getNavigationEvents('End', 'Error', 'Cancel');
  /* Observable<NavigationCancel | NavigationEnd | NavigationError> */
}
```

`RouterEvents` also delivers improved type-safety. In the example above, `navigationFinish$` has inferred type of `Observable<NavigationCancel | NavigationEnd | NavigationError>` whereas it would have `Observable<Event>` when router events are filtered directly.

## Usage

You do not have to provide `RouterEvents` at the module or component level, because it is already **provided in root**. You can inject and start using it immediately in your components.

### How to Get Specific Navigation Events

You can use `getNavigationEvents` to get a stream of navigation events matching given event keys.

```js
import { RouterEvents } from '@abp/ng.core';
import { merge } from 'rxjs';
import { mapTo } from 'rxjs/operators';

@Injectable()
class SomeService {
  private routerEvents = inject(RouterEvents);

  navigationStart$ = this.routerEvents.getNavigationEvents('Start');
  /* Observable<NavigationStart> */

  navigationFinish$ = this.routerEvents.getNavigationEvents('End', 'Error', 'Cancel');
  /* Observable<NavigationCancel | NavigationEnd | NavigationError> */

  loading$ = merge(
    this.navigationStart$.pipe(mapTo(true)),
    this.navigationFinish$.pipe(mapTo(false)),
  );
  /* Observable<boolean> */
}
```

### How to Get All Navigation Events

You can use `getAllNavigationEvents` to get a stream of all navigation events without passing any keys.

```js
import { RouterEvents, NavigationStart } from '@abp/ng.core';
import { map } from 'rxjs/operators';

@Injectable()
class SomeService {
  private routerEvents = inject(RouterEvents);

  navigationEvent$ = this.routerEvents.getAllNavigationEvents();
  /* Observable<NavigationCancel | NavigationEnd | NavigationError | NavigationStart> */

  loading$ = this.navigationEvent$.pipe(
    map(event => event instanceof NavigationStart),
  );
  /* Observable<boolean> */
}
```

### How to get Current and Previous Navigation

You can use `previousNavigation` and `currentNavigation` properties to retrieve navigations in a reactive way.

```ts
previousNavigation: Signal<string>;
currentNavigation: Signal<string>;
```

```ts
import { RouterEvents } from "@abp/ng.core";

@Injectable()
class SomeService {
  readonly routerEvents = inject(RouterEvents);

  someAction() {
    const previousNavUrl = this.routerEvents.previousNavigation();
    if (previousNavUrl) {
      // perform some action
    }
  }
}
```

### How to Get Specific Router Events

You can use `getEvents` to get a stream of router events matching given event classes.

```js
import { RouterEvents } from '@abp/ng.core';
import { ActivationEnd, ChildActivationEnd } from '@angular/router';

@Injectable()
class SomeService {
  private routerEvents = inject(RouterEvents);

  moduleActivation$ = this.routerEvents.getEvents(ActivationEnd, ChildActivationEnd);
  /* Observable<ActivationEnd | ChildActivationEnd> */
}
```

### How to Get All Router Events

You can use `getEvents` to get a stream of all router events without passing any event classes. This is nothing different from accessing `events` property of `Router` and is added to the service just for convenience.

```js
import { RouterEvents } from '@abp/ng.core';
import { ActivationEnd, ChildActivationEnd } from '@angular/router';

@Injectable()
class SomeService {
  private routerEvents = inject(RouterEvents);

  routerEvent$ = this.routerEvents.getAllEvents();
  /* Observable<Event> */
}
```
