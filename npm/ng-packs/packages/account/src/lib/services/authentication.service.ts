import { Inject, Injectable, Optional } from '@angular/core';
import {
  AbpApplicationConfigurationService,
  AuthPasswordFlowStrategy,
  AuthService,
  ConfigStateService,
  RestService,
  SessionStateService,
} from '@abp/ng.core';
import { from, Observable } from 'rxjs';
import { OAuthService } from 'angular-oauth2-oidc';
import { HttpHeaders } from '@angular/common/http';
import { switchMap, take, tap } from 'rxjs/operators';
import { ACCOUNT_OPTIONS } from '../tokens/options.token';
import { Options } from '../models/options';
import snq from 'snq';
import { Router } from '@angular/router';

@Injectable()
export class AuthenticationService {
  constructor(
    protected rest: RestService,
    protected sessionState: SessionStateService,
    protected authService: AuthService,
    protected oAuthService: OAuthService,
    protected appConfigService: AbpApplicationConfigurationService,
    protected configState: ConfigStateService,
    @Inject(ACCOUNT_OPTIONS) protected options: Options,
    protected router: Router,
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
          snq(() => window.history.state.redirectUrl) || (this.options || {}).redirectUrl || '/';

        this.router.navigate([redirectUrl]);

        const strategy = this.authService.activeAuthFlowStrategy;
        if (strategy instanceof AuthPasswordFlowStrategy) strategy.setRememberMe(remember);
      }),
      take(1),
    );
  }
}
