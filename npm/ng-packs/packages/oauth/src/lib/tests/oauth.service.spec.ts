import { DOCUMENT, PLATFORM_ID } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { of, Subject } from 'rxjs';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { APP_STARTED_WITH_SSR, CORE_OPTIONS, EnvironmentService } from '@abp/ng.core';
import { AbpOAuthService } from '../services/oauth.service';
import { MemoryTokenStorageService } from '../services/memory-token-storage.service';
import { AUTH_FLOW_STRATEGY } from '../tokens/auth-flow-strategy';

describe('AbpOAuthService', () => {
  let strategy: {
    init: any;
    startLogin: any;
    login: any;
    usesDefaultRedirect: any;
    isInternalAuth: boolean;
  };
  let oAuthService: {
    hasValidAccessToken: any;
    getAccessTokenExpiration: any;
    events: Subject<{ type: string }>;
  };
  let storage: MemoryTokenStorageService & {
    markSignInStarted: any;
    markSignInFinished: any;
  };
  let resolveStorage: () => void;

  function create(isInternalAuth = false, options: { skipInitAuthService?: boolean } = {}) {
    strategy = {
      init: vi.fn(() => Promise.resolve()),
      // The default code flow redirect returns a promise; other ways to sign in return nothing.
      startLogin: vi.fn(() => (isInternalAuth ? undefined : new Promise<void>(() => {}))),
      login: vi.fn(() => of(null)),
      usesDefaultRedirect: vi.fn(() => !isInternalAuth),
      isInternalAuth,
    };
    vi.spyOn(AUTH_FLOW_STRATEGY, 'Code').mockReturnValue(strategy as any);
    vi.spyOn(AUTH_FLOW_STRATEGY, 'Password').mockReturnValue(strategy as any);
    oAuthService = {
      hasValidAccessToken: vi.fn(() => false),
      getAccessTokenExpiration: vi.fn(() => Date.now() + 60_000),
      events: new Subject(),
    };

    storage = Object.create(MemoryTokenStorageService.prototype);
    (storage as any).ready = new Promise<void>(resolve => (resolveStorage = resolve));
    storage.markSignInStarted = vi.fn();
    storage.markSignInFinished = vi.fn();

    TestBed.configureTestingModule({
      providers: [
        { provide: APP_STARTED_WITH_SSR, useValue: false },
        { provide: PLATFORM_ID, useValue: 'browser' },
        { provide: DOCUMENT, useValue: document },
        { provide: OAuthService, useValue: oAuthService },
        { provide: OAuthStorage, useValue: storage },
        { provide: CORE_OPTIONS, useValue: { skipInitAuthService: !!options.skipInitAuthService } },
        {
          provide: EnvironmentService,
          useValue: {
            getEnvironment$: () =>
              of({ oAuthConfig: { responseType: isInternalAuth ? 'password' : 'code' } }),
          },
        },
      ],
    });
    return TestBed.inject(AbpOAuthService);
  }

  const flush = () => new Promise(resolve => setTimeout(resolve));

  afterEach(() => {
    vi.restoreAllMocks();
    TestBed.resetTestingModule();
  });

  it('should wait for the stored tokens before initializing the strategy', async () => {
    const service = create();

    const init = service.init();
    await flush();
    expect(strategy.init).not.toHaveBeenCalled();

    resolveStorage();
    await init;
    expect(strategy.init).toHaveBeenCalledTimes(1);
  });

  it('should start the sign-in after init when it is requested before init', async () => {
    const service = create();

    service.navigateToLogin({ returnUrl: '/page' });
    expect(storage.markSignInStarted).toHaveBeenCalled();
    await flush();
    expect(strategy.startLogin).not.toHaveBeenCalled();

    resolveStorage();
    await service.init();
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledWith({ returnUrl: '/page' });
  });

  it('should keep the sign-in state while the shared redirect is in progress', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    const redirect = new Promise<void>(() => {});
    // The code flow strategy returns the same redirect to parallel requests.
    strategy.startLogin.mockReturnValue(redirect);

    service.navigateToLogin({ returnUrl: '/first' });
    service.navigateToLogin({ returnUrl: '/second' });
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledTimes(2);
    expect(storage.markSignInStarted).toHaveBeenCalled();
    expect(storage.markSignInFinished).toHaveBeenCalledTimes(1); // only by init()
  });

  it('should skip the sign-in when the tab got a token while it waited', async () => {
    const service = create();
    service.navigateToLogin({ returnUrl: '/page' });

    oAuthService.hasValidAccessToken.mockReturnValue(true);
    resolveStorage();
    await service.init();
    await flush();

    expect(strategy.startLogin).not.toHaveBeenCalled();
    expect(storage.markSignInFinished).toHaveBeenCalled();
  });

  it('should start an early sign-in requested while the tab had a valid token', async () => {
    const service = create();
    oAuthService.hasValidAccessToken.mockReturnValue(true);
    service.navigateToLogin({ prompt: 'login' });

    resolveStorage();
    await service.init();
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledWith({ prompt: 'login' });
  });

  it('should start an explicit early sign-in even when the tab got a token while it waited', async () => {
    const service = create();
    service.navigateToLogin({ returnUrl: '/page' });
    service.navigateToLogin({ prompt: 'login' });

    oAuthService.hasValidAccessToken.mockReturnValue(true);
    resolveStorage();
    await service.init();
    await flush();

    expect(strategy.startLogin.mock.calls).toEqual([[{ prompt: 'login' }]]);
  });

  it('should pass every early request to a custom way of opening the login page', async () => {
    const service = create();
    strategy.usesDefaultRedirect.mockReturnValue(false);
    strategy.startLogin.mockReturnValue(undefined);
    service.navigateToLogin({ returnUrl: '/first' });
    service.navigateToLogin({ returnUrl: '/second' });

    oAuthService.hasValidAccessToken.mockReturnValue(true);
    resolveStorage();
    await service.init();
    await flush();

    expect(strategy.startLogin.mock.calls).toEqual([
      [{ returnUrl: '/first' }],
      [{ returnUrl: '/second' }],
    ]);
  });

  it('should end the sign-in state when the page is still here after the redirect', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    let stayed!: () => void;
    strategy.startLogin.mockReturnValueOnce(new Promise<void>(resolve => (stayed = resolve)));

    service.navigateToLogin();
    stayed();
    await flush();
    service.navigateToLogin();

    expect(storage.markSignInFinished).toHaveBeenCalledTimes(2); // by init() and the redirect
    expect(strategy.startLogin).toHaveBeenCalledTimes(2);
  });

  it('should start an early sign-in when the token expired within the allowed clock skew', async () => {
    const service = create();
    service.navigateToLogin({ returnUrl: '/page' });

    oAuthService.hasValidAccessToken.mockReturnValue(true);
    oAuthService.getAccessTokenExpiration.mockReturnValue(Date.now() - 60_000);
    resolveStorage();
    await service.init();
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledWith({ returnUrl: '/page' });
  });

  it('should start a sign-in requested after init even with a valid token', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    oAuthService.hasValidAccessToken.mockReturnValue(true);

    service.navigateToLogin({ prompt: 'login' });
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledWith({ prompt: 'login' });
  });

  it('should allow a new sign-in when starting the previous one failed', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    strategy.startLogin.mockImplementationOnce(() => {
      throw new Error('failed');
    });
    vi.spyOn(console, 'error').mockImplementation(() => {});

    service.navigateToLogin();
    await flush();
    service.navigateToLogin();
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledTimes(2);
  });

  it('should allow a new sign-in when the login page could not be opened', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    strategy.startLogin.mockReturnValueOnce(Promise.reject(new Error('failed')));
    vi.spyOn(console, 'error').mockImplementation(() => {});

    service.navigateToLogin();
    await flush();
    service.navigateToLogin();
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledTimes(2);
  });

  it('should pass every request to a custom way of opening the login page', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    strategy.startLogin.mockReturnValue(undefined);

    service.navigateToLogin();
    await flush();
    service.navigateToLogin();
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledTimes(2);
  });

  it('should pass requests to a custom way of opening the login page while init runs', async () => {
    const service = create();
    let finishInit!: () => void;
    strategy.init.mockReturnValue(new Promise<void>(resolve => (finishInit = resolve)));
    strategy.usesDefaultRedirect.mockReturnValue(false);
    strategy.startLogin.mockReturnValue(undefined);
    oAuthService.hasValidAccessToken.mockReturnValue(true);
    resolveStorage();
    const init = service.init();
    await flush();

    service.navigateToLogin({ prompt: 'login' });
    service.navigateToLogin({ returnUrl: '/second' });
    finishInit();
    await init;

    expect(strategy.startLogin.mock.calls).toEqual([
      [{ prompt: 'login' }],
      [{ returnUrl: '/second' }],
    ]);
  });

  it('should pass requests of the same turn to a custom way of opening the login page', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    strategy.startLogin.mockReturnValue(undefined);

    service.navigateToLogin({ returnUrl: '/first' });
    service.navigateToLogin({ returnUrl: '/second' });

    expect(strategy.startLogin.mock.calls).toEqual([
      [{ returnUrl: '/first' }],
      [{ returnUrl: '/second' }],
    ]);
  });

  it('should call the login of the strategy', async () => {
    const service = create();
    resolveStorage();
    await service.init();
    strategy.login.mockReturnValue(of('custom'));

    let result: unknown;
    service.login({ username: 'admin' } as any).subscribe(value => (result = value));

    expect(strategy.login).toHaveBeenCalledWith({ username: 'admin' });
    expect(result).toBe('custom');
  });

  it('should not wait for init when the app skips it', async () => {
    const service = create(false, { skipInitAuthService: true });

    service.navigateToLogin({ returnUrl: '/page' });
    await flush();

    // There is no strategy without init, so there is nothing to start.
    expect(strategy.startLogin).not.toHaveBeenCalled();
    expect(storage.markSignInFinished).toHaveBeenCalled();
  });

  it('should end the sign-in state of a callback that gave no token', async () => {
    const service = create();
    resolveStorage();

    await service.init();

    expect(storage.markSignInFinished).toHaveBeenCalled();
  });

  it('should allow later sign-ins with the password flow', async () => {
    const service = create(true);
    resolveStorage();
    await service.init();

    service.navigateToLogin();
    await flush();
    service.navigateToLogin();
    await flush();

    expect(strategy.startLogin).toHaveBeenCalledTimes(2);
  });
});
