import { Injectable, Injector } from '@angular/core';
import { Params } from '@angular/router';
import { from, Observable, lastValueFrom } from 'rxjs';
import { filter, map, switchMap, take, tap } from 'rxjs/operators';
import { IAuthService, LoginParams } from '@abp/ng.core';
import { AuthFlowStrategy } from '../strategies';
import { EnvironmentService } from '@abp/ng.core';
import { AUTH_FLOW_STRATEGY } from '../tokens/auth-flow-strategy';
import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({
  providedIn: 'root',
})
export class AbpOAuthService implements IAuthService {
  private strategy: AuthFlowStrategy;

  get isInternalAuth() {
    return this.strategy.isInternalAuth;
  }

  constructor(protected injector: Injector, private oAuthService: OAuthService) {}

  async init() {
    const environmentService = this.injector.get(EnvironmentService);

    const result$ = environmentService.getEnvironment$().pipe(
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
    );

    return await lastValueFrom(result$);
  }

  logout(queryParams?: Params): Observable<any> {
    return this.strategy.logout(queryParams);
  }

  navigateToLogin(queryParams?: Params) {
    this.strategy.navigateToLogin(queryParams);
  }

  login(params: LoginParams) {
    return this.strategy.login(params);
  }

  get isAuthenticated(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }
}
