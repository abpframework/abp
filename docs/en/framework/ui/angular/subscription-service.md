# Managing RxJS Subscriptions

`SubscriptionService` is a utility service to provide an easy unsubscription from RxJS observables in Angular components and directives. Please see [why you should unsubscribe from observables on instance destruction](https://angular.io/guide/lifecycle-hooks#cleaning-up-on-instance-destruction).

## Getting Started

You have to provide the `SubscriptionService` at component or directive level, because it is **not provided in root** and it works in sync with component/directive lifecycle. Only after then you can inject and start using it.

```js
import { SubscriptionService } from '@abp/ng.core';

@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent {
  count$ = interval(1000);

  constructor(private subscription: SubscriptionService) {
    this.subscription.addOne(this.count$, console.log);
  }
}
```

The values emitted by the `count$` will be logged until the component is destroyed. You will not have to unsubscribe manually.

> Please do not try to use a singleton `SubscriptionService`. It simply will not work.

## Usage

### How to Subscribe to Observables

You can pass a `next` function and an `error` function.

```js
@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent implements OnInit {
  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    const source$ = interval(1000);
    const nextFn = value => console.log(value * 2);
    const errorFn = error => {
      console.error(error);
      return of(null);
    };

    this.subscription.addOne(source$, nextFn, errorFn);
  }
}
```

Or, you can pass an observer.

```js
@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent implements OnInit {
  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    const source$ = interval(1000);
    const observer = {
      next: value => console.log(value * 2),
      complete: () => console.log('DONE'),
    };

    this.subscription.addOne(source$, observer);
  }
}
```

The `addOne` method returns the individual subscription, so that you may use it later on. Please see topics below for details.

### How to Unsubscribe Before Instance Destruction

There are two ways to do that. If you are not going to subscribe again, you may use the `closeAll` method.

```js
@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent implements OnInit {
  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    this.subscription.addOne(interval(1000), console.log);
  }

  onSomeEvent() {
    this.subscription.closeAll();
  }
}
```

This will clear all subscriptions, but you will not be able to subscribe again. If you are planning to add another subscription, you may use the `reset` method instead.

```js
@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent implements OnInit {
  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    this.subscription.addOne(interval(1000), console.log);
  }

  onSomeEvent() {
    this.subscription.reset();
    this.subscription.addOne(interval(1000), console.warn);
  }
}
```

### How to Unsubscribe From a Single Subscription

Sometimes, you may need to unsubscribe from a particular subscription but leave others alive. In such a case, you may use the `closeOne` method.

```js
@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent implements OnInit {
  countSubscription: Subscription;

  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    this.countSubscription = this.subscription.addOne(
      interval(1000),
      console.log
    );
  }

  onSomeEvent() {
    this.subscription.closeOne(this.countSubscription);
    console.log(this.countSubscription.closed); // true
  }
}
```

### How to Remove a Single Subscription From Tracked Subscriptions

You may want to take control of a particular subscription. In such a case, you may use the `removeOne` method to remove it from tracked subscriptions.

```js
@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent implements OnInit {
  countSubscription: Subscription;

  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    this.countSubscription = this.subscription.addOne(
      interval(1000),
      console.log
    );
  }

  onSomeEvent() {
    this.subscription.removeOne(this.countSubscription);
    console.log(this.countSubscription.closed); // false
  }
}
```

### How to Check If Unsubscribed From All

Please use `isClosed` getter to check if `closeAll` was called before.

```js
@Component({
  /* class metadata here */
  providers: [SubscriptionService],
})
class DemoComponent implements OnInit {
  constructor(private subscription: SubscriptionService) {}

  ngOnInit() {
    this.subscription.addOne(interval(1000), console.log);
  }

  onSomeEvent() {
    console.log(this.subscription.isClosed); // false
  }
}
```
