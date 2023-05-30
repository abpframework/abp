import { AuthService } from '@abp/ng.core';
import { Injectable } from '@angular/core';


@Injectable()
export class AuthenticationFlowGuard  {
  constructor(private authService: AuthService) {}

  canActivate() {
    if (this.authService.isInternalAuth) return true;

    this.authService.navigateToLogin();
    return false;
  }
}
