# HTTP Error Reporter Service

`HttpErrorReporterService` is a service which is exposed by `@abp/ng.core` package. HTTP errors can be reported by using this service. The service emits an event when an error is reported and keeps the errors as an array. The [`RestService`](./HTTP-Requests#restservice) uses the `HttpErrorReporterService` for reporting errors.

See the example below to learn how to report an error:

```ts
import { HttpErrorReporterService } from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class SomeService {
  constructor(private http: HttpClient, private httpErrorReporter: HttpErrorReporterService) {}

  getData() {
    return this.http.get('http://example.com/get-data').pipe(
      catchError(err => {
        this.httpErrorReporter.reportError(err);
        return of(null);
      }),
    );
  }
}
```

See the following example to learn listening the reported errors:

```ts
import { HttpErrorReporterService } from '@abp/ng.core';
import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class MyErrorHandler {
  constructor(private httpErrorReporter: HttpErrorReporterService) {
    this.handleErrors();
  }

  handleErrors() {
    this.httpErrorReporter.reporter$.subscribe((err: HttpErrorResponse) => {
        // handle the errors here
    });
  }
}
```


## API


### `reporter$: Observable<HttpErrorResponse>`

`reporter$` is a getter, returns an observable. It emits an event when a new error is reported. The event value type is `HttpErrorResponse`.


### `errors$: Observable<HttpErrorResponse[]>`

`errors$` is a getter, returns an observable. It emits an event when a new error is reported. The event value is all errors reported at runtime.

### `errors: HttpErrorResponse`

`errors` is a getter that returns all errors reported.


### `reportError(error: HttpErrorResponse): void`

`reportError` is a method. The errors can be reported via this. 
When an error is reported, the method triggers the `reports$` and `errors$` observables to emit an event.