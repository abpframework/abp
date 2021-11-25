# 轻松取消将你的观察取消订阅

`SubscriptionService` 是一个实用工具服务,它提供了一个简单的取消订阅Angular组件和指令中的RxJS可观察对象的功能. 请参见[为什么在实例销毁时要取消订阅可观察对象](https://angular.io/guide/lifecycle-hooks#cleaning-up-on-instance-destruction).

## 入门

你必须在组件或指令级别提供 `SubscriptionService`,因为它没有在**根中提供**,而且它与组件/指令的生命周期同步. 只有在此之后，您才能注入并开始使用它。

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

由 `count$` 发出的值将被记录下来,直到组件被销毁. 你不必手动退订.

> 请不要尝试使用单例 `SubscriptionService`. 这是行不通的.

## 用法

## 如何订阅可观察对象

你可以传递 `next` 函数和 `error` 函数.



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

或者,你可以传递一个观察者.

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

`addOne` 方法返回单个订阅以便你以后使用它. 有关详细信息,请参见下面的主题.

### 实例销毁之前如何退订

有两种方法可以做到这一点. 如果你不想再次订阅.可以使用 `closeAll` 方法.

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

这将清除所有订阅,你将无法再次订阅. 如果你打算添加另一个订阅,可以使用`reset`方法.

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

### 如何取消单个订阅

有时你可能需要取消订阅特定的订阅,但保留其他订阅. 在这种情况下,你可以使用 `closeOne` 方法.

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

### 如何从跟踪的订阅中删除单个订阅

你可能需要控制特定的订阅. 在这种情况下你可以使用 `removeOne` 方法将其从跟踪的订阅中删除.

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

### 如何检查是否全部取消订阅

使用 `isClosed` 检查 `closeAll` 是否被调用.

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

## 下一步是什么?

- [ListService](./List-Service.md)
