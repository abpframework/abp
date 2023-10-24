import { createServiceFactory, SpectatorService, createSpyObject } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { AbpOAuthGuard } from '../guards/oauth.guard';
import { AuthService } from '@abp/ng.core';
import { ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

describe('AuthGuard', () => {
  let spectator: SpectatorService<AbpOAuthGuard>;
  let guard : AbpOAuthGuard;
  const route = createSpyObject<ActivatedRouteSnapshot>(ActivatedRouteSnapshot)
  const state = createSpyObject<RouterStateSnapshot>(RouterStateSnapshot)

  const createService = createServiceFactory({
    service: AbpOAuthGuard,
    mocks: [OAuthService, AuthService],
  });

  beforeEach(() => {
    spectator = createService();
    guard = spectator.service;
  });

  it('should return true when user logged in', () => {
    spectator.inject(OAuthService).hasValidAccessToken.andReturn(true);
    expect(guard.canActivate(route, state)).toBe(true);
  });

  it('should execute the navigateToLogin method of the authService', () => {
    const authService = spectator.inject(AuthService);
    spectator.inject(OAuthService).hasValidAccessToken.andReturn(false);
    const navigateToLoginSpy = jest.spyOn(authService, 'navigateToLogin');

    expect(guard.canActivate(route, state)).toBe(false);
    expect(navigateToLoginSpy).toHaveBeenCalled();
  });
});
