import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { ConfigStateService } from '@abp/ng.core';

@Injectable()
export class ManageProfileGuard implements CanActivate {
  constructor(private configState: ConfigStateService) {}

  canActivate(_: ActivatedRouteSnapshot, __: RouterStateSnapshot) {
    const env = this.configState.getEnvironment();
    if (env.oAuthConfig.responseType === 'code') {
      window.open(`${env.oAuthConfig.issuer}/Account/Manage`, '_blank');
      return false;
    } else {
      return true;
    }
  }
}
