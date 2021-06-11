import { Injectable, Injector } from '@angular/core';
import { Params } from '@angular/router';
import { from, Observable } from 'rxjs';
import { filter, map, switchMap, take, tap } from 'rxjs/operators';
import { LoginParams } from '../models/auth';
import { AuthFlowStrategy, AUTH_FLOW_STRATEGY } from '../strategies/auth-flow.strategy';
import { EnvironmentService } from './environment.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private strategy: AuthFlowStrategy;

  get isInternalAuth() {
    return this.strategy.isInternalAuth;
  }

  constructor(protected injector: Injector) {}

  async init() {
    const environmentService = this.injector.get(EnvironmentService);

    return environmentService
      .getEnvironment$()
      .pipe(
        map(env => env?.oAuthConfig),
        filter(oAuthConfig => !!oAuthConfig),
        tap(oAuthConfig => {
          this.strategy =
            oAuthConfig.responseType === 'code'
              ? AUTH_FLOW_STRATEGY.Code(this.injector)
              : AUTH_FLOW_STRATEGY.Password(this.injector);
        }),
        switchMap(() => from(this.strategy.init())),
        take(1),
      )
      .toPromise();
  }

  logout(): Observable<any> {
    return this.strategy.logout();
  }

  /**
   * @deprecated Use navigateToLogin method instead. To be deleted in v5.0
   */
  initLogin() {
    this.strategy.navigateToLogin();
  }

  navigateToLogin(queryParams?: Params) {
    this.strategy.navigateToLogin(queryParams);
  }

  login(params: LoginParams) {
    return this.strategy.login(params);
  }
}
