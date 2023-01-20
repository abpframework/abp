import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { AbpOAuthGuard } from '../guards/oauth.guard';
import { AuthService } from '@Abp/ng.core';

describe('AuthGuard', () => {
  let spectator: SpectatorService<AbpOAuthGuard>;
  let guard: AbpOAuthGuard;
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
    expect(guard.canActivate()).toBe(true);
  });

  it('should execute the navigateToLogin method of the authService', () => {
    const authService = spectator.inject(AuthService);
    spectator.inject(OAuthService).hasValidAccessToken.andReturn(false);
    const navigateToLoginSpy = jest.spyOn(authService, 'navigateToLogin');

    expect(guard.canActivate()).toBe(false);
    expect(navigateToLoginSpy).toHaveBeenCalled();
  });
});
