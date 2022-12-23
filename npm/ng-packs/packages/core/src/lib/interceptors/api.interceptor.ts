import {
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ApiInterceptor implements IApiInterceptor {
  getAdditionalHeaders(existingHeaders?: HttpHeaders) {
    return existingHeaders;
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req);
  }
}

export interface IApiInterceptor extends HttpInterceptor {
  getAdditionalHeaders(existingHeaders?: HttpHeaders): HttpHeaders;
}
