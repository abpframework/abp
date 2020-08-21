import { HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, Injector, Optional } from '@angular/core';
import { Navigate } from '@ngxs/router-plugin';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, Observable } from 'rxjs';
import { switchMap, take, tap } from 'rxjs/operators';
import snq from 'snq';
import { GetAppConfiguration, SetEnvironment } from '../actions/config.actions';
import { ConfigState } from '../states/config.state';
import { SessionState } from '../states/session.state';
import { AuthFlowStrategy, AUTH_FLOW_STRATEGY } from '../strategies/auth-flow.strategy';
import { RestService } from './rest.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private flow: string;
  private strategy: AuthFlowStrategy;

  get isInternalAuth() {
    return this.strategy.isInternalAuth;
  }

  constructor(
    private actions: Actions,
    private injector: Injector,
    private rest: RestService,
    private oAuthService: OAuthService,
    private store: Store,
    @Optional() @Inject('ACCOUNT_OPTIONS') private options: any,
  ) {
    this.setStrategy();
    this.listenToSetEnvironment();
  }

  private setStrategy = () => {
    const flow =
      this.store.selectSnapshot(ConfigState.getDeep('environment.oAuthConfig.responseType')) ||
      'password';
    if (this.flow === flow) return;

    if (this.strategy) this.strategy.destroy();

    this.flow = flow;
    this.strategy =
      this.flow === 'code'
        ? AUTH_FLOW_STRATEGY.Code(this.injector)
        : AUTH_FLOW_STRATEGY.Password(this.injector);
  };

  private listenToSetEnvironment() {
    this.actions.pipe(ofActionSuccessful(SetEnvironment)).subscribe(this.setStrategy);
  }

  login(username: string, password: string): Observable<any> {
    const tenant = this.store.selectSnapshot(SessionState.getTenant);

    return from(
      this.oAuthService.fetchTokenUsingPasswordFlow(
        username,
        password,
        new HttpHeaders({ ...(tenant && tenant.id && { __tenant: tenant.id }) }),
      ),
    ).pipe(
      switchMap(() => this.store.dispatch(new GetAppConfiguration())),
      tap(() => {
        const redirectUrl =
          snq(() => window.history.state.redirectUrl) || (this.options || {}).redirectUrl || '/';
        this.store.dispatch(new Navigate([redirectUrl]));
      }),
      take(1),
    );
  }

  async init() {
    return await this.strategy.init();
  }

  logout(): Observable<any> {
    return this.strategy.logout();
  }

  initLogin() {
    this.strategy.login();
  }
}
