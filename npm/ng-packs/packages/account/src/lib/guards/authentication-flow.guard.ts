import { AuthService, IAbpGuard } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

/**
 * @deprecated Use `authenticationFlowGuard` instead.
 */
@Injectable()
export class AuthenticationFlowGuard implements IAbpGuard {
  protected readonly authService = inject(AuthService);

  canActivate() {
    if (this.authService.isInternalAuth) return true;

    this.authService.navigateToLogin();
    return false;
  }
}

export const authenticationFlowGuard = () => {
  const authService = inject(AuthService);

  if (authService.isInternalAuth) return true;

  authService.navigateToLogin();
  return false;
};
