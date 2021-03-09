import {
  AbpApplicationConfigurationService,
  AuthPasswordFlowStrategy,
  AuthService,
  ConfigStateService,
  RestService,
  SessionStateService,
} from '@abp/ng.core';
import { HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { from, Observable } from 'rxjs';
import { switchMap, take, tap } from 'rxjs/operators';
import { AccountConfigOptions } from '../models/config-options';
import { ACCOUNT_CONFIG_OPTIONS } from '../tokens/config-options.token';

@Injectable()
export class AuthenticationService {
  constructor(
    protected rest: RestService,
    protected sessionState: SessionStateService,
    protected authService: AuthService,
    protected oAuthService: OAuthService,
    protected appConfigService: AbpApplicationConfigurationService,
    protected configState: ConfigStateService,
    @Inject(ACCOUNT_CONFIG_OPTIONS) protected options: AccountConfigOptions,
    protected router: Router,
    protected route: ActivatedRoute,
  ) {}

  login(username: string, password: string, remember = false): Observable<any> {
    const tenant = this.sessionState.getTenant();

    return from(
      this.oAuthService.fetchTokenUsingPasswordFlow(
        username,
        password,
        new HttpHeaders({ ...(tenant && tenant.id && { __tenant: tenant.id }) }),
      ),
    ).pipe(
      switchMap(() => this.appConfigService.get()),
      tap(res => {
        this.configState.setState(res);

        const redirectUrl =
          this.route.snapshot.queryParams.returnUrl || this.options.redirectUrl || '/';

        this.router.navigate([redirectUrl]);

        const strategy = this.authService.strategy;
        if (strategy instanceof AuthPasswordFlowStrategy) strategy.setRememberMe(remember);
      }),
    );
  }
}
