import {
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
  HttpEvent,
} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { HttpWaitService } from '../services';

@Injectable({
  providedIn: 'root',
})
export class ApiInterceptor implements IApiInterceptor {
  private httpWaitService = inject(HttpWaitService);

  getAdditionalHeaders(existingHeaders?: HttpHeaders) {
    return existingHeaders || new HttpHeaders();
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.httpWaitService.addRequest(request);
    return next.handle(request).pipe(finalize(() => this.httpWaitService.deleteRequest(request)));
  }
}

export interface IApiInterceptor extends HttpInterceptor {
  getAdditionalHeaders(existingHeaders?: HttpHeaders): HttpHeaders;
}
