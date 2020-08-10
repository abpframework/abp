import { Injectable, Injector } from '@angular/core';
import { CanActivate } from '@angular/router';
import { OAUTH_STRATEGY } from '@abp/ng.core';

@Injectable()
export class AuthenticationFlowGuard implements CanActivate {
  constructor(private injector: Injector) {}

  canActivate() {
    return OAUTH_STRATEGY.CanActivate(this.injector);
  }
}
