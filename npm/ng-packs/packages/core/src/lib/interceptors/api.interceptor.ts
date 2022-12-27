import { HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { HttpWaitService } from '../services';

@Injectable({
  providedIn: 'root',
})
export class ApiInterceptor implements IApiInterceptor {
  constructor(private httpWaitService: HttpWaitService) {}

  getAdditionalHeaders(existingHeaders?: HttpHeaders) {
    return existingHeaders;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    this.httpWaitService.addRequest(request);
    return next.handle(request).pipe(finalize(() => this.httpWaitService.deleteRequest(request)));
  }
}

export interface IApiInterceptor extends HttpInterceptor {
  getAdditionalHeaders(existingHeaders?: HttpHeaders): HttpHeaders;
}
