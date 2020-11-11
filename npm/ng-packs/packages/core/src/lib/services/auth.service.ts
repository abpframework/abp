import { HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, Injector, Optional } from '@angular/core';
import { Navigate } from '@ngxs/router-plugin';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, Observable } from 'rxjs';
import { switchMap, take, tap } from 'rxjs/operators';
import snq from 'snq';
import { AuthFlowStrategy, AUTH_FLOW_STRATEGY } from '../strategies/auth-flow.strategy';
import { ApplicationConfigurationService } from './application-configuration.service';
import { ConfigStateService } from './config-state.service';
import { EnvironmentService } from './environment.service';
import { SessionStateService } from './session-state.service';

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
    private environment: EnvironmentService,
    private injector: Injector,
    private oAuthService: OAuthService,
    private store: Store,
    private sessionState: SessionStateService,
    private configState: ConfigStateService,
    private appConfigService: ApplicationConfigurationService,
    @Optional() @Inject('ACCOUNT_OPTIONS') private options: any,
  ) {
    this.setStrategy();
    this.listenToSetEnvironment();
  }

  private setStrategy = () => {
    const flow = this.environment.getEnvironment().oAuthConfig.responseType || 'password';
    if (this.flow === flow) return;

    if (this.strategy) this.strategy.destroy();

    this.flow = flow;
    this.strategy =
      this.flow === 'code'
        ? AUTH_FLOW_STRATEGY.Code(this.injector)
        : AUTH_FLOW_STRATEGY.Password(this.injector);
  };

  private listenToSetEnvironment() {
    this.environment.createOnUpdateStream(state => state.oAuthConfig).subscribe(this.setStrategy);
  }

  login(username: string, password: string): Observable<any> {
    const tenant = this.sessionState.getTenant();

    return from(
      this.oAuthService.fetchTokenUsingPasswordFlow(
        username,
        password,
        new HttpHeaders({ ...(tenant && tenant.id && { __tenant: tenant.id }) }),
      ),
    ).pipe(
      switchMap(() =>
        this.appConfigService.getConfiguration().pipe(tap(res => this.configState.setState(res))),
      ),
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
