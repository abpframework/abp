import { inject, makeStateKey, PLATFORM_ID, TransferState } from '@angular/core';
import {
  HttpEvent,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';
import { tap } from 'rxjs/operators';

export const transferStateInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn,
): Observable<HttpEvent<any>> => {
  const transferState = inject(TransferState);
  const platformId = inject(PLATFORM_ID);

  if (req.method !== 'GET') {
    return next(req);
  }

  const stateKey = makeStateKey<HttpResponse<any>>(req.urlWithParams);

  if (isPlatformBrowser(platformId)) {
    const storedResponse = transferState.get<HttpResponse<any>>(stateKey, null);
    if (storedResponse) {
      transferState.remove(stateKey);
      return of(new HttpResponse<any>({ body: storedResponse, status: 200 }));
    }
  }

  return next(req).pipe(
    tap(event => {
      if (isPlatformServer(platformId) && event instanceof HttpResponse) {
        transferState.set(stateKey, event.body);
        console.log(`Interceptor: ${req.urlWithParams} is stored in TransferState.`);
      }
    }),
  );
};
