import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';
import { Store } from '@ngxs/store';
import { SessionState } from '../states';
import { StartLoader, StopLoader } from '../actions/loader.actions';
import { finalize } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ApiInterceptor implements HttpInterceptor {
  constructor(private oAuthService: OAuthService, private store: Store) {}

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    this.store.dispatch(new StartLoader(request));

    return next
      .handle(
        request.clone({
          setHeaders: this.getAdditionalHeaders(request.headers),
        }),
      )
      .pipe(finalize(() => this.store.dispatch(new StopLoader(request))));
  }

  getAdditionalHeaders(existingHeaders?: HttpHeaders) {
    const headers = {} as any;

    const token = this.oAuthService.getAccessToken();
    if (!existingHeaders?.has('Authorization') && token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const lang = this.store.selectSnapshot(SessionState.getLanguage);
    if (!existingHeaders?.has('Accept-Language') && lang) {
      headers['Accept-Language'] = lang;
    }

    const tenant = this.store.selectSnapshot(SessionState.getTenant);
    if (!existingHeaders?.has('__tenant') && tenant) {
      headers['__tenant'] = tenant.id;
    }

    return headers;
  }
}
