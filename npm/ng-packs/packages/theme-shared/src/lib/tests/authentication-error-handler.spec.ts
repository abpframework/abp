import {
  APP_BASE_HREF,
  HashLocationStrategy,
  Location,
  LocationStrategy,
  PathLocationStrategy,
  PlatformLocation,
} from '@angular/common';
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { AuthService, ConfigStateService } from '@abp/ng.core';
import { AbpAuthenticationErrorHandler } from '../services/authentication-error-handler.service';
import { HTTP_ERROR_CONFIG } from '../tokens/http-error.token';

describe('AbpAuthenticationErrorHandler', () => {
  let authService: {
    isAuthenticated: boolean;
    getAccessToken: any;
    getAccessTokenExpiration: any;
    navigateToLogin: any;
    logout: any;
  };
  let configState: { refreshAppState: any };

  // `accessToken` is '' (none), 'expired', 'skew' (expired, but within the allowed clock skew)
  // or a valid token.
  function create(
    accessToken: string,
    isAuthenticated = false,
    path = '/my-tasks?q=1#item',
    signInWithoutToken?: boolean,
  ) {
    authService = {
      isAuthenticated: !!accessToken && accessToken !== 'expired',
      getAccessToken: vi.fn(() => accessToken),
      getAccessTokenExpiration: vi.fn(() =>
        accessToken === 'expired'
          ? Date.now() - 3_600_000
          : accessToken === 'skew'
            ? Date.now() - 60_000
            : accessToken
              ? Date.now() + 60_000
              : 0,
      ),
      navigateToLogin: vi.fn(),
      logout: vi.fn(() => of(null)),
    };
    configState = {
      refreshAppState: vi.fn(() => of({ currentUser: { isAuthenticated } })),
    };
    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: authService },
        { provide: ConfigStateService, useValue: configState },
        { provide: HTTP_ERROR_CONFIG, useValue: { signInWithoutToken } },
        {
          provide: Location,
          useValue: { path: (includeHash: boolean) => (includeHash ? path : '') },
        },
      ],
    });
    return TestBed.inject(AbpAuthenticationErrorHandler);
  }

  afterEach(() => TestBed.resetTestingModule());

  it('should sign out as before when there is no token', () => {
    create('').execute();

    expect(configState.refreshAppState).toHaveBeenCalledTimes(1);
    expect(authService.logout).toHaveBeenCalledWith({ noRedirectToLogoutUrl: true });
    expect(authService.navigateToLogin).not.toHaveBeenCalled();
  });

  it('should sign in and come back to the current page without a token when it is enabled', () => {
    create('', false, '/my-tasks?q=1#item', true).execute();

    expect(authService.navigateToLogin).toHaveBeenCalledWith({ returnUrl: '/my-tasks?q=1#item' });
    expect(configState.refreshAppState).not.toHaveBeenCalled();
    expect(authService.logout).not.toHaveBeenCalled();
  });

  it('should sign in again when the token expired', () => {
    create('expired').execute();

    expect(authService.navigateToLogin).toHaveBeenCalledWith({ returnUrl: '/my-tasks?q=1#item' });
    expect(authService.logout).not.toHaveBeenCalled();
  });

  it('should sign in again when the token expired within the allowed clock skew', () => {
    create('skew').execute();

    expect(authService.navigateToLogin).toHaveBeenCalledWith({ returnUrl: '/my-tasks?q=1#item' });
    expect(configState.refreshAppState).not.toHaveBeenCalled();
  });

  it('should come back to the root page when the app path is empty', () => {
    create('expired', false, '').execute();

    expect(authService.navigateToLogin).toHaveBeenCalledWith({ returnUrl: '/' });
  });

  describe('with the Angular location', () => {
    function returnUrlFor(strategy: unknown, baseHref: string, url: Partial<Location>) {
      const navigateToLogin = vi.fn();
      const platformLocation = {
        pathname: '/',
        search: '',
        hash: '',
        ...url,
        getBaseHrefFromDOM: () => baseHref,
        onPopState: () => () => {},
        onHashChange: () => () => {},
      };
      TestBed.configureTestingModule({
        providers: [
          {
            provide: AuthService,
            useValue: { isAuthenticated: false, getAccessToken: () => '', navigateToLogin },
          },
          { provide: ConfigStateService, useValue: {} },
          { provide: HTTP_ERROR_CONFIG, useValue: { signInWithoutToken: true } },
          { provide: PlatformLocation, useValue: platformLocation },
          { provide: APP_BASE_HREF, useValue: baseHref },
          { provide: LocationStrategy, useClass: strategy as any },
        ],
      });
      TestBed.inject(AbpAuthenticationErrorHandler).execute();
      return navigateToLogin.mock.calls[0][0].returnUrl;
    }

    it('should leave out the base href', () => {
      const returnUrl = returnUrlFor(PathLocationStrategy, '/portal/', {
        pathname: '/portal/my-tasks',
        search: '?q=1',
      } as any);

      expect(returnUrl).toBe('/my-tasks?q=1');
    });

    it('should use the route of the hash', () => {
      const returnUrl = returnUrlFor(HashLocationStrategy, '/', { hash: '#/my-tasks?q=1' } as any);

      expect(returnUrl).toBe('/my-tasks?q=1');
    });
  });

  it('should sign out when the token is no longer accepted', () => {
    create('token').execute();

    // Loaded again first, in case the anonymous result came from an older token.
    expect(configState.refreshAppState).toHaveBeenCalledTimes(2);
    expect(authService.logout).toHaveBeenCalledWith({ noRedirectToLogoutUrl: true });
    expect(authService.navigateToLogin).not.toHaveBeenCalled();
  });

  it('should keep the token when a configuration of an older token comes first', () => {
    const handler = create('token');
    configState.refreshAppState
      .mockReturnValueOnce(of({ currentUser: { isAuthenticated: false } }))
      .mockReturnValueOnce(of({ currentUser: { isAuthenticated: true } }));

    handler.execute();

    expect(configState.refreshAppState).toHaveBeenCalledTimes(2);
    expect(authService.logout).not.toHaveBeenCalled();
  });

  it('should keep a token that another tab got while the configuration loaded', () => {
    const handler = create('old-token');
    authService.getAccessToken.mockReturnValueOnce('old-token').mockReturnValue('new-token');

    handler.execute();

    expect(configState.refreshAppState).toHaveBeenCalled();
    expect(authService.logout).not.toHaveBeenCalled();
  });

  it('should do nothing when the user is still authenticated', () => {
    create('token', true).execute();

    expect(authService.logout).not.toHaveBeenCalled();
    expect(authService.navigateToLogin).not.toHaveBeenCalled();
  });
});
