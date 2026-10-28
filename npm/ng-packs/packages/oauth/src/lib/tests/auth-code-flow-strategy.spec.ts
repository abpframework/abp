import { Injector } from '@angular/core';
import { Location } from '@angular/common';
import { Params } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { Subject } from 'rxjs';
import { AuthConfig, OAuthErrorEvent, OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { AuthService } from '@abp/ng.core';
// Loaded before the strategies, in the same order as the package entry point.
import '../services';
import { AuthCodeFlowStrategy } from '../strategies/auth-code-flow-strategy';
import { AuthFlowStrategy } from '../strategies/auth-flow-strategy';
import { AuthPasswordFlowStrategy } from '../strategies/auth-password-flow-strategy';
import { MemoryTokenStorageService } from '../services/memory-token-storage.service';
import * as clearOAuthStorageModule from '../utils/clear-o-auth-storage';

// App strategies written for the `void` signature still compile (checked by the spec type check).
class CustomCodeFlowStrategy extends AuthCodeFlowStrategy {
  override navigateToLogin(queryParams?: Params): void {
    return super.navigateToLogin(queryParams);
  }
}

class RouterCodeFlowStrategy extends AuthCodeFlowStrategy {
  override navigateToLogin(): Promise<boolean> {
    return Promise.resolve(true);
  }
}

describe('AuthCodeFlowStrategy', () => {
  let oAuthService: any;
  let strategy: any;

  beforeEach(() => {
    oAuthService = Object.create(OAuthService.prototype);
    oAuthService.loginUrl = 'https://auth.example/connect/authorize';
    // A spy that looks like the default openUri.
    const openUri = vi.fn();
    openUri.toString = () => String(new AuthConfig().openUri);
    oAuthService.config = { ...new AuthConfig(), openUri, requireHttps: true };
    oAuthService.createLoginUrl = vi.fn(async () => 'https://auth.example/login');
    strategy = Object.create(AuthCodeFlowStrategy.prototype);
    strategy.platformId = 'browser';
    strategy.appStartedWithSSR = false;
    strategy.sessionState = { getLanguage: () => 'en' };
    strategy.oAuthService = oAuthService;
    strategy.oAuthConfig = {};
    strategy.document = document;
    strategy.injector = { get: () => null };
  });

  afterEach(() => {
    vi.useRealTimers();
    vi.restoreAllMocks();
  });

  const flush = () => new Promise(resolve => setTimeout(resolve));

  it('should open the login page with the return URL', async () => {
    strategy.startLogin({ returnUrl: '/my-tasks' });
    await flush();

    expect(oAuthService.createLoginUrl).toHaveBeenCalledWith(
      '/my-tasks',
      '',
      '',
      false,
      expect.objectContaining({ returnUrl: '/my-tasks', culture: 'en' }),
    );
    expect(oAuthService.config.openUri).toHaveBeenCalledWith('https://auth.example/login');
  });

  it('should share one redirect between parallel requests and login()', async () => {
    let open!: () => void;
    oAuthService.createLoginUrl = vi.fn(
      () => new Promise<string>(resolve => (open = () => resolve('https://auth.example/login'))),
    );

    const first = strategy.startLogin({ returnUrl: '/first' });
    const second = strategy.startLogin({ returnUrl: '/second' });
    strategy.login();
    open();
    await flush();
    strategy.startLogin({ returnUrl: '/third' });
    await flush();

    expect(second).toBe(first);
    expect(oAuthService.createLoginUrl).toHaveBeenCalledTimes(1);
    expect(oAuthService.config.openUri).toHaveBeenCalledTimes(1);
  });

  it('should open the login page with the public API when the library lacks its members', async () => {
    const initCodeFlow = vi
      .spyOn(OAuthService.prototype, 'initCodeFlow')
      .mockImplementation(() => {});
    oAuthService.createLoginUrl = undefined;

    await strategy.startLogin({ returnUrl: '/my-tasks' });
    await strategy.startLogin({ returnUrl: '/my-tasks' });

    expect(initCodeFlow).toHaveBeenCalledTimes(2);
    expect(initCodeFlow).toHaveBeenCalledWith(
      '/my-tasks',
      expect.objectContaining({ culture: 'en' }),
    );
  });

  it('should start a new redirect after the previous one failed', async () => {
    oAuthService.createLoginUrl.mockRejectedValueOnce(new Error('PKCE failed'));

    await expect(strategy.startLogin()).rejects.toThrow('PKCE failed');
    strategy.startLogin();
    await flush();

    expect(oAuthService.createLoginUrl).toHaveBeenCalledTimes(2);
  });

  it('should keep the redirect shared while the login page loads', async () => {
    const first = strategy.startLogin();
    await flush();
    await new Promise(resolve => setTimeout(resolve, 50));

    expect(strategy.startLogin()).toBe(first);
    expect(oAuthService.createLoginUrl).toHaveBeenCalledTimes(1);
  });

  it('should keep the redirect shared when the user uses the page while the login page loads', async () => {
    const first = strategy.startLogin();
    await flush();

    window.dispatchEvent(new Event('beforeunload', { cancelable: true }));
    window.dispatchEvent(new Event('keydown'));
    await flush();

    expect(strategy.startLogin()).toBe(first);
    expect(oAuthService.createLoginUrl).toHaveBeenCalledTimes(1);
  });

  it('should start a new redirect when the user stays on the page at a beforeunload prompt', async () => {
    // The page asks before it leaves, and the user stays.
    const askBeforeLeaving = (event: Event) => event.preventDefault();
    window.addEventListener('beforeunload', askBeforeLeaving);
    oAuthService.config.openUri.mockImplementation(() =>
      window.dispatchEvent(new Event('beforeunload', { cancelable: true })),
    );
    const first = strategy.startLogin();
    await flush();
    window.removeEventListener('beforeunload', askBeforeLeaving);

    window.dispatchEvent(new Event('pointerdown'));
    await first;
    strategy.startLogin();
    await flush();

    expect(oAuthService.createLoginUrl).toHaveBeenCalledTimes(2);
    expect(oAuthService.config.openUri).toHaveBeenCalledTimes(2);
  });

  it('should start a new redirect when a later beforeunload listener asked and the user stayed', async () => {
    const askBeforeLeaving = (event: Event) => event.preventDefault();
    oAuthService.config.openUri.mockImplementationOnce(() => {
      // Added after the redirect started, so it runs after the listener of the strategy.
      window.addEventListener('beforeunload', askBeforeLeaving);
      window.dispatchEvent(new Event('beforeunload', { cancelable: true }));
      window.removeEventListener('beforeunload', askBeforeLeaving);
    });
    const first = strategy.startLogin();
    await flush();

    window.dispatchEvent(new Event('keydown'));
    await first;
    strategy.startLogin();
    await flush();

    expect(oAuthService.createLoginUrl).toHaveBeenCalledTimes(2);
  });

  it('should mark the sign-in of login() so tokens from other tabs do not reload the page', async () => {
    const memoryStorage = Object.create(MemoryTokenStorageService.prototype);
    memoryStorage.markSignInStarted = vi.fn();
    memoryStorage.markSignInFinished = vi.fn();
    strategy.injector = {
      get: (token: unknown) => (token === OAuthStorage ? memoryStorage : null),
    };

    strategy.login();
    await flush();
    expect(memoryStorage.markSignInStarted).toHaveBeenCalled();
    expect(memoryStorage.markSignInFinished).not.toHaveBeenCalled();

    window.dispatchEvent(Object.assign(new Event('pageshow'), { persisted: true }));
    await flush();
    expect(memoryStorage.markSignInFinished).toHaveBeenCalled();
  });

  it('should start a new redirect when the page comes back from the back-forward cache', async () => {
    const first = strategy.startLogin();
    await flush();

    const pageShow = new Event('pageshow');
    Object.defineProperty(pageShow, 'persisted', { value: true });
    window.dispatchEvent(pageShow);
    await first;
    strategy.startLogin();
    await flush();

    expect(oAuthService.createLoginUrl).toHaveBeenCalledTimes(2);
  });

  it('should pass every request to a custom openUri', () => {
    const initCodeFlow = vi
      .spyOn(OAuthService.prototype, 'initCodeFlow')
      .mockImplementation(() => {});
    // Like OAuthService.configure({ ...config, openUri }) after init.
    oAuthService.config = { ...new AuthConfig(), openUri: () => {} };

    expect(strategy.startLogin()).toBeUndefined();
    expect(strategy.startLogin()).toBeUndefined();
    expect(initCodeFlow).toHaveBeenCalledTimes(2);
  });

  it('should reject when the login page can not be opened', async () => {
    oAuthService.createLoginUrl.mockRejectedValue(new Error('PKCE failed'));

    await expect(strategy.startLogin()).rejects.toThrow('PKCE failed');
  });

  it('should keep a custom initCodeFlow', () => {
    oAuthService.initCodeFlow = vi.fn();

    expect(strategy.startLogin({ returnUrl: '/my-tasks' })).toBeUndefined();
    expect(oAuthService.initCodeFlow).toHaveBeenCalledWith('/my-tasks', expect.any(Object));
    expect(oAuthService.createLoginUrl).not.toHaveBeenCalled();
  });

  it('should keep the navigateToLogin of a subclass', () => {
    const custom: any = Object.create(RouterCodeFlowStrategy.prototype);
    Object.assign(custom, strategy);

    expect(custom.startLogin()).toBeUndefined();
    expect(oAuthService.createLoginUrl).not.toHaveBeenCalled();
    expect(Object.create(CustomCodeFlowStrategy.prototype)).toBeInstanceOf(AuthCodeFlowStrategy);
  });

  it('should pass the return URL to the server when the app started with SSR', () => {
    const replace = vi.fn();
    strategy.appStartedWithSSR = true;
    strategy.document = { defaultView: { location: { replace } } };

    strategy.navigateToLogin({ returnUrl: '/my-tasks?q=1' });

    expect(replace).toHaveBeenCalledWith('/authorize?returnUrl=%2Fmy-tasks%3Fq%3D1');
  });
});

describe('AuthFlowStrategy errors', () => {
  let clearSpy: ReturnType<typeof vi.spyOn>;
  let storage: any;
  let strategy: any;

  beforeEach(() => {
    clearSpy = vi.spyOn(clearOAuthStorageModule, 'clearOAuthStorage').mockImplementation(() => {});
    storage = Object.create(MemoryTokenStorageService.prototype);
    storage.hasOwnToken = false;
    storage.removeItem = vi.fn();
    strategy = Object.create(AuthCodeFlowStrategy.prototype);
    strategy.injector = Injector.create({
      providers: [{ provide: OAuthStorage, useValue: storage }],
    });
  });

  afterEach(() => vi.restoreAllMocks());

  it('should keep the shared tokens when a sign-in callback of this tab fails', () => {
    strategy.clearAfterError(new OAuthErrorEvent('invalid_nonce_in_state', null));

    expect(clearSpy).not.toHaveBeenCalled();
    expect(storage.removeItem.mock.calls.map(([key]: string[]) => key)).toEqual([
      'nonce',
      'PKCE_verifier',
      'session_state',
    ]);
  });

  for (const type of [AuthCodeFlowStrategy, AuthPasswordFlowStrategy]) {
    it(`${type.name} should not sign out the expired tokens of other tabs for the remember-me option`, () => {
      const flow: any = Object.create(type.prototype);
      flow.injector = strategy.injector;
      flow.rememberMeService = {
        get: () => false,
        set: vi.fn(),
        getFromToken: () => false,
        remove: vi.fn(),
      };
      flow.oAuthService = {
        getAccessToken: () => 'shared-token',
        getAccessTokenExpiration: () => Date.now() - 60_000,
        logOut: vi.fn(),
      };

      flow.checkRememberMeOption();
      expect(flow.oAuthService.logOut).not.toHaveBeenCalled();

      // The tab's own expired token is signed out as before.
      storage.hasOwnToken = true;
      flow.checkRememberMeOption();
      expect(flow.oAuthService.logOut).toHaveBeenCalledTimes(1);
    });
  }

  it('should clear the shared tokens when the logout of this tab fails to revoke them', () => {
    const reason = new HttpErrorResponse({ status: 0 });

    strategy.clearAfterError(new OAuthErrorEvent('token_revoke_error', reason));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });

  it('should clear the tokens when this tab stored tokens before the error', () => {
    // For example the ID token of a sign-in callback failed the validation.
    storage.hasOwnToken = true;

    strategy.clearAfterError(new OAuthErrorEvent('token_validation_error', null));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });

  it('should clear the tokens when the refresh token is rejected', () => {
    strategy.clearAfterError(new OAuthErrorEvent('token_refresh_error', null));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });

  for (const status of [0, 503]) {
    it(`should keep the tokens when a refresh fails with status ${status}`, () => {
      const reason = new HttpErrorResponse({ status });

      strategy.clearAfterError(new OAuthErrorEvent('token_refresh_error', reason));

      expect(clearSpy).not.toHaveBeenCalled();
      expect(storage.removeItem).not.toHaveBeenCalled();
    });
  }

  it('should clear the tokens when the server rejects the refresh token', () => {
    const reason = new HttpErrorResponse({ status: 400, error: { error: 'invalid_grant' } });

    strategy.clearAfterError(new OAuthErrorEvent('token_refresh_error', reason));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });

  for (const type of [AuthCodeFlowStrategy, AuthPasswordFlowStrategy]) {
    it(`${type.name} should keep the tokens when its refresh can't reach the server`, async () => {
      const refreshing: any = Object.create(type.prototype);
      refreshing.injector = strategy.injector;
      refreshing.rememberMeService = { remove: vi.fn() };
      refreshing.oAuthService = {
        refreshToken: () => Promise.reject(new HttpErrorResponse({ status: 0 })),
      };

      await refreshing.refreshToken();

      expect(clearSpy).not.toHaveBeenCalled();
      expect(refreshing.rememberMeService.remove).not.toHaveBeenCalled();
    });

    it(`${type.name} should clear the tokens when its refresh token is rejected`, async () => {
      const refreshing: any = Object.create(type.prototype);
      refreshing.injector = strategy.injector;
      refreshing.rememberMeService = { remove: vi.fn() };
      refreshing.oAuthService = {
        refreshToken: () => Promise.reject(new HttpErrorResponse({ status: 400 })),
      };

      await refreshing.refreshToken();

      expect(clearSpy).toHaveBeenCalledTimes(1);
    });
  }

  it('should clear the tokens of other storages as before', () => {
    strategy.injector = Injector.create({
      providers: [{ provide: OAuthStorage, useValue: { removeItem: vi.fn() } }],
    });

    strategy.clearAfterError(new OAuthErrorEvent('invalid_nonce_in_state', null));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });

  it('should start the sign-in through AuthService after a rejected refresh', async () => {
    const events = new Subject<any>();
    const navigateToLogin = vi.fn();
    strategy.injector = Injector.create({
      providers: [
        { provide: AuthService, useValue: { navigateToLogin } },
        { provide: Location, useValue: { path: () => '/my-tasks?q=1' } },
      ],
    });
    strategy.oAuthConfig = {};
    strategy.navigateToPreviousUrl = () => {};
    strategy.catchError = () => {};
    strategy.oAuthService = {
      configure: () => {},
      events,
      loadDiscoveryDocument: () => Promise.resolve(),
      getAccessTokenExpiration: () => Date.now() + 60_000,
      getRefreshToken: () => null,
    };
    await AuthFlowStrategy.prototype.init.call(strategy);

    events.next(new OAuthErrorEvent('token_refresh_error', new HttpErrorResponse({ status: 0 })));
    expect(navigateToLogin).not.toHaveBeenCalled();

    events.next(new OAuthErrorEvent('token_refresh_error', new HttpErrorResponse({ status: 400 })));
    expect(navigateToLogin).toHaveBeenCalledWith({ returnUrl: '/my-tasks?q=1' });
  });
});
