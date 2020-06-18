import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { AuthGuard } from '../guards/auth.guard';
import { OAuthService } from 'angular-oauth2-oidc';
import { RouterModule, UrlTree, Router } from '@angular/router';
import { RouterOutletComponent } from '../components';
import { APP_BASE_HREF } from '@angular/common';

describe('AuthGuard', () => {
  let spectator: SpectatorService<AuthGuard>;
  let guard: AuthGuard;
  const createService = createServiceFactory({
    service: AuthGuard,
    mocks: [OAuthService, Router],
    imports: [RouterModule.forRoot([{ path: '', component: RouterOutletComponent }])],
    declarations: [RouterOutletComponent],
    providers: [{ provide: APP_BASE_HREF, useValue: '/' }],
  });

  beforeEach(() => {
    spectator = createService();
    guard = spectator.service;
  });

  it('should return true when user logged in', () => {
    spectator.get(OAuthService).hasValidAccessToken.andReturn(true);
    expect(guard.canActivate(null, null)).toBe(true);
  });

  it('should return navigate to login page with redirectUrl state', () => {
    const router = spectator.get(Router);
    spectator.get(OAuthService).hasValidAccessToken.andReturn(false);

    expect(guard.canActivate(null, { url: '/' } as any)).toBe(true);
    expect(router.navigate).toHaveBeenCalledWith(['/account/login'], {
      state: { redirectUrl: '/' },
    });
  });
});
