import { AuthService, IAbpGuard } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable()
export class AuthenticationFlowGuard implements IAbpGuard {
  protected readonly authService = inject(AuthService);

  canActivate() {
    if (this.authService.isInternalAuth) return true;

    this.authService.navigateToLogin();
    return false;
  }
}
