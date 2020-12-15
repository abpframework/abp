import { HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { finalize } from 'rxjs/operators';
import { StartLoader, StopLoader } from '../actions/loader.actions';
import { SessionStateService } from '../services/session-state.service';

@Injectable({
  providedIn: 'root',
})
export class ApiInterceptor implements HttpInterceptor {
  constructor(
    private oAuthService: OAuthService,
    private store: Store,
    private sessionState: SessionStateService,
  ) {}

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

    const lang = this.sessionState.getLanguage();
    if (!existingHeaders?.has('Accept-Language') && lang) {
      headers['Accept-Language'] = lang;
    }

    const tenant = this.sessionState.getTenant();
    if (!existingHeaders?.has('__tenant') && tenant?.id) {
      headers['__tenant'] = tenant.id;
    }

    return headers;
  }
}
