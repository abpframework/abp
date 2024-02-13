import { Injectable, Injector } from '@angular/core';
import { Params } from '@angular/router';
import { from, Observable, lastValueFrom, EMPTY } from 'rxjs';
import { filter, map, switchMap, take, tap } from 'rxjs/operators';
import { AbpAuthResponse, IAuthService, LoginParams } from '@abp/ng.core';
import { AuthFlowStrategy } from '../strategies';
import { EnvironmentService } from '@abp/ng.core';
import { AUTH_FLOW_STRATEGY } from '../tokens/auth-flow-strategy';
import { OAuthService } from 'angular-oauth2-oidc';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AbpOAuthService implements IAuthService {
  private strategy!: AuthFlowStrategy;
  private readonly oAuthService: OAuthService;

  get oidc() {
    return this.oAuthService.oidc;
  }

  set oidc(value) {
    this.oAuthService.oidc = value;
  }

  get isInternalAuth() {
    return this.strategy.isInternalAuth;
  }

  constructor(protected injector: Injector) {
    this.oAuthService = this.injector.get(OAuthService);
  }

  async init() {
    const environmentService = this.injector.get(EnvironmentService);

    const result$ = environmentService.getEnvironment$().pipe(
      map(env => env?.oAuthConfig),
      filter(Boolean),
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
    if (!this.strategy) {
      return EMPTY;
    }

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

  loginUsingGrant(
    grantType: string,
    parameters: object,
    headers?: HttpHeaders,
  ): Promise<AbpAuthResponse> {
    const { clientId: client_id, dummyClientSecret: client_secret } = this.oAuthService;
    const access_token = this.oAuthService.getAccessToken();
    const p = {
      access_token,
      grant_type: grantType,
      client_id,
      ...parameters,
    };

    if (client_secret) {
      p['client_secret'] = client_secret;
    }

    return this.oAuthService.fetchTokenUsingGrant(grantType, p, headers);
  }

  getRefreshToken(): string {
    return this.oAuthService.getRefreshToken();
  }

  getAccessToken(): string {
    return this.oAuthService.getAccessToken();
  }

  refreshToken(): Promise<AbpAuthResponse> {
    return this.oAuthService.refreshToken();
  }

  getAccessTokenExpiration(): number {
    return this.oAuthService.getAccessTokenExpiration();
  }
}
