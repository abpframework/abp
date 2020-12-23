import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthGuard } from '../guards/auth.guard';
import { AuthService } from '../services/auth.service';

describe('AuthGuard', () => {
  let spectator: SpectatorService<AuthGuard>;
  let guard: AuthGuard;
  const createService = createServiceFactory({
    service: AuthGuard,
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

  it('should execute the initLogin method of the authService', () => {
    const authService = spectator.inject(AuthService);
    spectator.inject(OAuthService).hasValidAccessToken.andReturn(false);
    const initLoginSpy = jest.spyOn(authService, 'initLogin');

    expect(guard.canActivate()).toBe(true);
    expect(initLoginSpy).toHaveBeenCalled();
  });
});
