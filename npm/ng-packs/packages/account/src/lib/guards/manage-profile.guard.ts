import { EnvironmentService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class ManageProfileGuard implements CanActivate {
  constructor(private environment: EnvironmentService) {}

  canActivate(_: ActivatedRouteSnapshot, __: RouterStateSnapshot) {
    const env = this.environment.getEnvironment();
    if (env.oAuthConfig.responseType === 'code') {
      window.location.href = `${env.oAuthConfig.issuer}/Account/Manage?returnUrl=${window.location.href}`;
      return false;
    } else {
      return true;
    }
  }
}
