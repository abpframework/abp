import { Injectable } from '@angular/core';
import {
  HttpErrorResponse,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { inject } from '@angular/core';
import { HTTP_TOASTER_INTERCEPTOR_CONFIG } from '../tokens/toaster-interceptor.token';
import { Toaster, HttpResponseEvent } from '../models';
import { ToasterService } from '../services';

@Injectable()
export class ToasterInterceptor implements HttpInterceptor {
  protected readonly toasterService = inject(ToasterService);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    const interceptorKey = Toaster.SKIP_TOASTER_INTERCEPTOR;

    if (req.headers.has(interceptorKey)) {
      req.headers.delete(interceptorKey);
      return next.handle(req);
    }

    const { defaults, methods } = inject(HTTP_TOASTER_INTERCEPTOR_CONFIG);

    if (!methods.includes(req.method as Toaster.HttpMethod)) {
      return next.handle(req);
    }

    return next.handle(req).pipe(
      tap(event => {
        if (event instanceof HttpResponse) {
          this.showToaster(event, defaults);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        this.showToaster(error, defaults);
        return throwError(error);
      }),
    );
  }

  private showToaster(event: HttpResponseEvent, defaults: Partial<Toaster.ToasterDefaults>) {
    const { status } = event;

    const toasterParams = defaults[status];
    if (!toasterParams) {
      console.error(`Toaster params not found for status code: ${status}`);
      return;
    }

    const { message, title, severity, options } = toasterParams;
    this.toasterService.show(message, title, severity, options);
  }
}
