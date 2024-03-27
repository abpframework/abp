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
import { Toaster } from '../models';
import { ToasterService } from '../services';

@Injectable()
export class ToasterInterceptor implements HttpInterceptor {
  protected readonly toasterService = inject(ToasterService);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    const { defaults, methods } = inject(HTTP_TOASTER_INTERCEPTOR_CONFIG);

    if (!methods.includes(req.method as Toaster.HttpMethod)) {
      return next.handle(req);
    }

    return next.handle(req).pipe(
      tap(event => {
        if (event instanceof HttpResponse) {
          const { status } = event;
          const defaultToaster = defaults[status];
          this.toasterService.show(defaultToaster);
        }
      }),
      catchError((error: HttpErrorResponse) => {
        const { status } = error;
        const defaultToaster = defaults[status];
        this.toasterService.show(defaultToaster);
        return throwError(error);
      }),
    );
  }
}
