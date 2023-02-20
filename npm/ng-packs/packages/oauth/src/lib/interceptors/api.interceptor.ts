import { HttpHandler, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { finalize } from 'rxjs/operators';
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
  constructor(
    private oAuthService: OAuthService,
    private sessionState: SessionStateService,
    private httpWaitService: HttpWaitService,
    @Inject(TENANT_KEY) private tenantKey: string,
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler) {
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
