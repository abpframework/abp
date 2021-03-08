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

@Injectable()
class SomeService {
  navigationFinish$ = this.router.events.pipe(
    filter(
      event =>
        event instanceof NavigationEnd ||
        event instanceof NavigationError ||
        event instanceof NavigationCancel,
    ),
  );
  /* Observable<Event> */

  constructor(private router: Router) {}
}
```

However, `RouterEvents` makes filtering router events easier.

```js
import { RouterEvents } from '@abp/ng.core';

@Injectable()
class SomeService {
  navigationFinish$ = this.routerEvents.getNavigationEvents('End', 'Error', 'Cancel');
  /* Observable<NavigationCancel | NavigationEnd | NavigationError> */

  constructor(private routerEvents: RouterEvents) {}
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
  navigationStart$ = this.routerEvents.getNavigationEvents('Start');
  /* Observable<NavigationStart> */

  navigationFinish$ = this.routerEvents.getNavigationEvents('End', 'Error', 'Cancel');
  /* Observable<NavigationCancel | NavigationEnd | NavigationError> */

  loading$ = merge(
    this.navigationStart$.pipe(mapTo(true)),
    this.navigationFinish$.pipe(mapTo(false)),
  );
  /* Observable<boolean> */

  constructor(private routerEvents: RouterEvents) {}
}
```


### How to Get All Navigation Events

You can use `getAllNavigationEvents` to get a stream of all navigation events without passing any keys.

```js
import { RouterEvents, NavigationStart } from '@abp/ng.core';
import { map } from 'rxjs/operators';

@Injectable()
class SomeService {
  navigationEvent$ = this.routerEvents.getAllNavigationEvents();
  /* Observable<NavigationCancel | NavigationEnd | NavigationError | NavigationStart> */

  loading$ = this.navigationEvent$.pipe(
    map(event => event instanceof NavigationStart),
  );
  /* Observable<boolean> */

  constructor(private routerEvents: RouterEvents) {}
}
```


### How to Get Specific Router Events

You can use `getEvents` to get a stream of router events matching given event constructors.

```js
import { RouterEvents } from '@abp/ng.core';
import { ActivationEnd, ChildActivationEnd } from '@angular/router';

@Injectable()
class SomeService {
  moduleActivation$ = this.routerEvents.getEvents(ActivationEnd, ChildActivationEnd);
  /* Observable<ActivationEnd | ChildActivationEnd> */

  constructor(private routerEvents: RouterEvents) {}
}
```


### How to Get All Router Events

You can use `getEvents` to get a stream of all router events without passing any event constructors. This is nothing different from accessing `events` property of `Router` and is added to the service just for convenience.

```js
import { RouterEvents } from '@abp/ng.core';
import { ActivationEnd, ChildActivationEnd } from '@angular/router';

@Injectable()
class SomeService {
  routerEvent$ = this.routerEvents.getAllEvents();
  /* Observable<Event> */

  constructor(private routerEvents: RouterEvents) {}
}
```

