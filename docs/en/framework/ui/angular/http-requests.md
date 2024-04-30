# How to Make HTTP Requests

## About HttpClient

Angular has the amazing [HttpClient](https://angular.io/guide/http) for communication with backend services. It is a layer on top and a simplified representation of [XMLHttpRequest Web API](https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest). It also is the recommended agent by Angular for any HTTP request. There is nothing wrong with using the `HttpClient` in your ABP project.

However, `HttpClient` leaves error handling to the caller (method). In other words, HTTP errors are handled manually and by hooking into the observer of the `Observable` returned.

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

Although clear and flexible, handling errors this way is repetitive work, even when error processing is delegated to the store or any other injectable.

An `HttpInterceptor` is able to catch `HttpErrorResponse` and can be used for a centralized error handling. Nevertheless, cases where default error handler, therefore the interceptor, must be disabled require additional work and comprehension of Angular internals. Check [this issue](https://github.com/angular/angular/issues/20203) for details.

## RestService

ABP core module has a utility service for HTTP requests: `RestService`. Unless explicitly configured otherwise, it catches HTTP errors and dispatches a `RestOccurError` action. This action is then captured by the `ErrorHandler` introduced by the `ThemeSharedModule`. Since you should already import this module in your app, when the `RestService` is used, all HTTP errors get automatically handled by default.

### Getting Started with RestService

In order to use the `RestService`, you must inject it in your class as a dependency.

```js
import { RestService } from '@abp/ng.core';

@Injectable({
  /* class metadata here */
})
class DemoService {
  constructor(private rest: RestService) {}
}
```

You do not have to provide the `RestService` at module or component/directive level, because it is already **provided in root**.

### How to Make a Request with RestService

You can use the `request` method of the `RestService` is for HTTP requests. Here is an example:

```js
getFoo(id: number) {
  const request: Rest.Request<null> = {
    method: 'GET',
    url: '/api/some/path/to/foo/' + id,
  };

  return this.rest.request<null, FooResponse>(request);
}
```

The `request` method always returns an `Observable<T>`. Therefore you can do the following wherever you use `getFoo` method:

```js
doSomethingWithFoo(id: number) {
  this.demoService.getFoo(id).subscribe(
    foo => {
      // Do something with foo.
    }
  )
}
```

**You do not have to worry about unsubscription.** The `RestService` uses `HttpClient` behind the scenes, so every observable it returns is a finite observable, i.e. it closes subscriptions automatically upon success or error.

As you see, `request` method gets a request options object with `Rest.Request<T>` type. This generic type expects the interface of the request body. You may pass `null` when there is no body, like in a `GET` or a `DELETE` request. Here is an example where there is one:

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

You may [check here](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/rest.ts#L23) for complete `Rest.Request<T>` type, which has only a few changes compared to [HttpRequest](https://angular.io/api/common/http/HttpRequest) class in Angular.

### How to Disable Default Error Handler of RestService

The `request` method, used with defaults, always handles errors. Let's see how you can change that behavior and handle errors yourself:

```js
deleteFoo(id: number) {
  const request: Rest.Request<null> = {
    method: 'DELETE',
    url: '/api/some/path/to/foo/' + id,
  };

  return this.rest.request<null, void>(request, { skipHandleError: true });
}
```

`skipHandleError` config option, when set to `true`, disables the error handler and the returned observable starts throwing an error that you can catch in your subscription.

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

### How to Get a Specific API Endpoint From Application Config

Another nice config option that `request` method receives is `apiName` (available as of v2.4), which can be used to get a specific module endpoint from application configuration.

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

`putFoo` above will request `https://localhost:44305/api/some/path/to/foo/{id}` as long as the environment variables are as follows:

```js
// environment.ts

export const environment = {
  apis: {
    default: {
      url: "https://localhost:44305",
    },
    foo: {
      url: "https://localhost:44305/api/some/path/to/foo",
    },
  },

  /* rest of the environment variables here */
};
```

### How to Observe Response Object or HTTP Events Instead of Body

`RestService` assumes you are generally interested in the body of a response and, by default, sets `observe` property as `'body'`. However, there may be times you are rather interested in something else, such as a custom proprietary header. For that, the `request` method receives `observe` property in its config object.

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

You may find `Rest.Observe` enum [here](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/rest.ts#L10).

### How to Skip HTTP interceptors and ABP headers

The ABP adds several HTTP headers to the HttpClient, such as the "Auth token" or "tenant Id".
The ABP Server must possess the information but the ABP user may not want to send this informations to an external server.
ExternalHttpClient and IS EXTERNAL REQUEST HttpContext Token were added in V6.0.4.
The ABP Http interceptors check the value of the `IS_EXTERNAL_REQUEST` token. If the token is True then ABP-specific headers won't be added to Http Request.
The `ExternalHttpClient` extends from `HTTPClient` and sets the `IS_EXTERNAL_REQUEST` context token to true.
When you are using `ExternalHttpClient` as HttpClient in your components, it does not add ABP-specific headers.

Note: With `IS_EXTERNAL_REQUEST` or without it, ABP loading service works.

## See Also

- [HTTP Error Handling / Customization](./http-error-handling.md)
