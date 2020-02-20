import { HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, Observable } from 'rxjs';
import { switchMap, tap, take } from 'rxjs/operators';
import snq from 'snq';
import { GetAppConfiguration } from '../actions/config.actions';
import { SessionState } from '../states/session.state';
import { RestService } from './rest.service';
import { ConfigState } from '../states/config.state';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private rest: RestService,
    private oAuthService: OAuthService,
    private store: Store,
    @Optional() @Inject('ACCOUNT_OPTIONS') private options: any,
  ) {}

  login(username: string, password: string): Observable<any> {
    const tenant = this.store.selectSnapshot(SessionState.getTenant);

    this.oAuthService.configure(
      this.store.selectSnapshot(ConfigState.getOne('environment')).oAuthConfig,
    );

    return from(this.oAuthService.loadDiscoveryDocument()).pipe(
      switchMap(() =>
        from(
          this.oAuthService.fetchTokenUsingPasswordFlow(
            username,
            password,
            new HttpHeaders({ ...(tenant && tenant.id && { __tenant: tenant.id }) }),
          ),
        ),
      ),
      switchMap(() => this.store.dispatch(new GetAppConfiguration())),
      tap(() => {
        const redirectUrl =
          snq(() => window.history.state.redirectUrl) || (this.options || {}).redirectUrl || '/';
        this.store.dispatch(new Navigate([redirectUrl]));
      }),
      take(1),
    );
  }

  logout(): Observable<void> {
    return this.rest
      .request({
        method: 'GET',
        url: '/api/account/logout',
      })
      .pipe(
        switchMap(() => {
          this.oAuthService.logOut();
          return this.store.dispatch(new GetAppConfiguration());
        }),
      );
  }
}
