import { HttpEvent, HttpHandler, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { finalize } from 'rxjs/operators';
import { Observable } from 'rxjs';
import {
  HttpWaitService,
  IApiInterceptor,
  IS_EXTERNAL_REQUEST,
  SessionStateService,
  TENANT_KEY,
} from '@abp/ng.core';

@Injectable({
  providedIn: 'root',
})
export class OAuthApiInterceptor implements IApiInterceptor {
  private oAuthService = inject(OAuthService);
  private sessionState = inject(SessionStateService);
  private httpWaitService = inject(HttpWaitService);
  private tenantKey = inject(TENANT_KEY);

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.httpWaitService.addRequest(request);
    const isExternalRequest = request.context?.get(IS_EXTERNAL_REQUEST);
    const newRequest = isExternalRequest
      ? request
      : request.clone({
          setHeaders: this.getAdditionalHeaders(request.headers),
        });

    return next
      .handle(newRequest)
      .pipe(finalize(() => this.httpWaitService.deleteRequest(request)));
  }

  getAdditionalHeaders(existingHeaders?: HttpHeaders) {
    const headers = {} as any;

    const token = this.oAuthService.getAccessToken();
    if (!existingHeaders?.has('Authorization') && token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const lang = this.sessionState.getLanguage();
    if (!existingHeaders?.has('Accept-Language') && lang) {
      headers['Accept-Language'] = lang;
    }

    const tenant = this.sessionState.getTenant();
    if (!existingHeaders?.has(this.tenantKey) && tenant?.id) {
      headers[this.tenantKey] = tenant.id;
    }

    headers['X-Requested-With'] = 'XMLHttpRequest';

    return headers;
  }
}
