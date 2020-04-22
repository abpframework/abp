## HTTP请求

## 关于 HttpClient

Angular具有很棒的 `HttpClient` 与后端服务进行通信. 它位于顶层,是[XMLHttpRequest Web API](https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest)的封装. 同时也是Angular建议用于任何HTTP请求的代理,在你的ABP项目中使用 `HttpClient` 是最佳做法.

但是 `HttpClient` 将错误处理留给调用方,换句话说HTTP错误是通过手动处理的,通过挂接到返回的 `Observable` 的观察者中来处理.

```js
getConfig() {
  this.http.get(this.configUrl).subscribe(
    config => this.updateConfig(config),
    error => {
      // Handle error here
    },
  );
}
```

上面的代码尽管清晰灵活,但即使将错误处理委派给Store或任何其他注入. 以这种方式处理错误也是重复性的工作.

`HttpInterceptor` 能够捕获 `HttpErrorResponse` 并可用于集中的错误处理. 然而,在必须放置错误处理程序(也就是拦截器)的情况下,需要额外的工作以及对Angular内部机制的理解. 检查[这个issue](https://github.com/angular/angular/issues/20203)了解详情.

## RestService

ABP核心模块有用于HTTP请求的实用程序服务: `RestService`. 除非另有明确配置,否则它将捕获HTTP错误并调度 `RestOccurError` 操作, 然后由 `ThemeSharedModule` 引入的 `ErrorHandler` 捕获此操作. 你应该已经在应用程序中导入了此模块,在使用 `RestService` 时,默认情况下将自动处理所有HTTP错误.

### RestService 入门

为了使用 `RestService`, 你必须将它注入到你的类中.

```js
import { RestService } from '@abp/ng.core';

@Injectable({
  /* class metadata here */
})
class DemoService {
  constructor(private rest: RestService) {}
}
```

你不必在模块或组件/指令级别提供 `estService`,因为它已经在**根中**中提供了.

### 如何使用RestService发出请求

你可以使用 `RestService` 的 `request` 方法来处理HTTP请求. 示例:

```js
getFoo(id: number) {
  const request: Rest.Request<null> = {
    method: 'GET',
    url: '/api/some/path/to/foo/' + id,
  };

  return this.rest.request<null, FooResponse>(request);
}
```

`request` 方法始终返回 `Observable<T>`. 无论何时使用 `getFoo` 方法,都可以执行以下操作:

```js
doSomethingWithFoo(id: number) {
  this.demoService.getFoo(id).subscribe(
    foo => {
      // Do something with foo.
    }
  )
}
```

**你不必担心关于取消订阅**. `RestService` 在内部使用 `HttpClient`,因此它返回的每个可观察对象都是有限的可观察对象,成功或出错后将自动关闭订阅.

如你所见,`request` 方法获取一个具有 `Rest.Reques<T>` 类型的请求选项对象. 此泛型类型需要请求主体的接口. 当没有正文时,例如在 `GET` 或 `DELETE` 请求中,你可以传递 `null`. 示例:

```js
postFoo(body: Foo) {
  const request: Rest.Request<Foo> = {
    method: 'POST',
    url: '/api/some/path/to/foo',
  	body
  };

  return this.rest.request<Foo, FooResponse>(request);
}
```

你可以在[此处检查](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/rest.ts#L23)完整的 `Rest.Request<T>` 类型,与Angular中的[HttpRequest](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/rest.ts#L23)类相比只有很少的改动.

### 如何禁用RestService的默认错误处理程序

默认 `request` 方法始终处理错误. 让我们看看如何改变这种行为并由自己处理错误:

```js
deleteFoo(id: number) {
  const request: Rest.Request<null> = {
    method: 'DELETE',
    url: '/api/some/path/to/foo/' + id,
  };

  return this.rest.request<null, void>(request, { skipHandleError: true });
}
```

`skipHandleError` 配置选项设置为 `true` 时,禁用错误处理程序,并返回 `observable` 引发错误,你可以在订阅中捕获该错误.

```js
removeFooFromList(id: number) {
  this.demoService.deleteFoo(id).subscribe(
    foo => {
      // Do something with foo.
    },
    error => {
      // Do something with error.
    }
  )
}
```

### 如何从应用程序配置获取特定的API端点

`request` 方法接收到的另一个配置选项是 `apiName` (在v2.4中可用),它用于从应用程序配置获取特定的模块端点.

```js
putFoo(body: Foo, id: string) {
  const request: Rest.Request<Foo> = {
    method: 'PUT',
    url: '/' + id,
  	body
  };

  return this.rest.request<Foo, void>(request, {apiName: 'foo'});
}
```

上面的putFoo将请求 `https://localhost:44305/api/some/path/to/foo/{id}` 当环境变量如下:

```js
// environment.ts

export const environment = {
  apis: {
    default: {
      url: 'https://localhost:44305',
    },
    foo: {
      url: 'https://localhost:44305/api/some/path/to/foo',
    },
  },
  
  /* rest of the environment variables here */
}
```

### 如何观察响应对象或HTTP事件而不是正文

`RestService` 假定你通常对响应的正文感兴趣,默认情况下将 `observe` 属性设置为 `body`. 但是有时你可能对其他内容(例如自定义标头)非常感兴趣. 为此, `request` 方法在 `config` 对象中接收 `watch` 属性.

```js
getSomeCustomHeaderValue() {
  const request: Rest.Request<null> = {
    method: 'GET',
    url: '/api/some/path/that/sends/some-custom-header',
  };

  return this.rest.request<null, HttpResponse<any>>(
    request,
    {observe: Rest.Observe.Response},
  ).pipe(
    map(response => response.headers.get('Some-Custom-Header'))
  );
}
```

你可以在[此处](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/rest.ts#L10)找到 `Rest.Observe` 枚举.

## 下一步是什么?

* [本地化](./Localization.md)