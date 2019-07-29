import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';
import { Store } from '@ngxs/store';
import { SessionState } from '../states';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  constructor(private oAuthService: OAuthService, private store: Store) {}

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    const headers = {} as any;
    const token = this.oAuthService.getAccessToken();
    if (!request.headers.has('Authorization') && token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const lang = this.store.selectSnapshot(SessionState.getLanguage);
    if (!request.headers.has('Accept-Language') && lang) {
      headers['Accept-Language'] = lang;
    }

    return next.handle(
      request.clone({
        setHeaders: headers,
      }),
    );
  }
}
