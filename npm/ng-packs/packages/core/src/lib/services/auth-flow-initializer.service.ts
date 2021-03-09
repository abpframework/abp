import { Injectable, Injector } from '@angular/core';
import { from } from 'rxjs';
import { filter, map, switchMap, take, tap } from 'rxjs/operators';
import { AUTH_FLOW_STRATEGY } from '../strategies/auth-flow.strategy';
import { AuthService } from './auth.service';
import { EnvironmentService } from './environment.service';

@Injectable()
export class AuthFlowInitializerService {
  constructor(
    protected injector: Injector,
    protected environmentService: EnvironmentService,
    protected authService: AuthService,
  ) {}

  init(): Promise<any> {
    return this.environmentService
      .getEnvironment$()
      .pipe(
        map(env => env?.oAuthConfig),
        filter(oAuthConfig => !!oAuthConfig),
        tap(oAuthConfig => {
          this.authService.strategy =
            oAuthConfig.responseType === 'code'
              ? AUTH_FLOW_STRATEGY.Code(this.injector)
              : AUTH_FLOW_STRATEGY.Password(this.injector);
        }),
        switchMap(() => from(this.authService.init())),
        take(1),
      )
      .toPromise();
  }
}
