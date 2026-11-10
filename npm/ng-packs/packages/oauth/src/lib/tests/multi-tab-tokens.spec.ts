import {
  createEnvironmentInjector,
  DOCUMENT,
  EnvironmentInjector,
  NgZone,
  runInInjectionContext,
} from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { OAuthService, OAuthStorage, UrlHelperService } from 'angular-oauth2-oidc';
import { AbpLocalStorageService, ConfigStateService } from '@abp/ng.core';
import { MemoryTokenStorageService } from '../services/memory-token-storage.service';
import { checkAccessToken } from '../utils/check-access-token';
import { clearOAuthStorage } from '../utils/clear-o-auth-storage';

// Runs the real worker script. Each tab has its own port, and the test decides when the worker
// handles the messages of a port and when the tab receives the answers.
class FakePort {
  toWorker: any[] = [];
  toTab: any[] = [];
  onmessage: ((event: { data: any }) => void) | null = null;
  workerSide = {
    onmessage: null as ((event: { data: any }) => void) | null,
    addEventListener: () => {},
    start: () => {},
    postMessage: (data: any) => this.toTab.push(data),
  };
  start() {}
  postMessage(data: any) {
    this.toWorker.push(data);
  }
  drain() {
    while (this.toWorker.length) {
      this.workerSide.onmessage?.({ data: this.toWorker.shift() });
    }
    while (this.toTab.length) {
      this.onmessage?.({ data: this.toTab.shift() });
    }
  }
}

class FakeSharedWorker {
  static scope: any;
  static last: FakeSharedWorker;
  port = new FakePort();
  listeners: Record<string, () => void> = {};
  constructor(url: string) {
    if (!FakeSharedWorker.scope) {
      FakeSharedWorker.scope = {};
      new Function('self', atob(url.split(',')[1]))(FakeSharedWorker.scope);
    }
    FakeSharedWorker.last = this;
    FakeSharedWorker.scope.onconnect({ ports: [this.port.workerSide] });
  }
  addEventListener(type: string, listener: () => void) {
    this.listeners[type] = listener;
  }
}

describe('Tokens shared by tabs', () => {
  const injectors: EnvironmentInjector[] = [];
  // The tabs of a browser share localStorage.
  let sharedLocal: Map<string, string>;

  function openTab(options: { search?: string; session?: Map<string, string> } = {}) {
    const session = options.session ?? new Map<string, string>();
    const reload = vi.fn();
    const injector = createEnvironmentInjector(
      [
        {
          provide: AbpLocalStorageService,
          useValue: {
            getItem: (key: string) => sharedLocal.get(key) ?? null,
            setItem: (key: string, value: string) => sharedLocal.set(key, value),
            removeItem: (key: string) => sharedLocal.delete(key),
          },
        },
        {
          provide: DOCUMENT,
          useValue: {
            defaultView: {
              location: { search: options.search ?? '', reload },
              sessionStorage: {
                getItem: (key: string) => session.get(key) ?? null,
                setItem: (key: string, value: string) => session.set(key, value),
                removeItem: (key: string) => session.delete(key),
              },
              addEventListener: () => {},
              removeEventListener: () => {},
            },
          },
        },
        // The application configuration of the tab is anonymous.
        { provide: ConfigStateService, useValue: { getDeep: () => null } },
      ],
      TestBed.inject(EnvironmentInjector),
    );
    injectors.push(injector);

    const storage = runInInjectionContext(injector, () => new MemoryTokenStorageService());
    const worker = FakeSharedWorker.last;
    const logger = { debug: vi.fn(), info: vi.fn(), log: vi.fn(), warn: vi.fn(), error: vi.fn() };
    const oAuthService = runInInjectionContext(
      injector,
      () =>
        new OAuthService(
          TestBed.inject(NgZone),
          null as any,
          storage,
          null as any,
          { oidc: false } as any,
          new UrlHelperService(),
          logger as any,
          null as any,
          document,
          { now: () => Date.now(), new: () => new Date() } as any,
        ),
    );
    const checkInjector = createEnvironmentInjector(
      [
        { provide: OAuthStorage, useValue: storage },
        { provide: OAuthService, useValue: oAuthService },
      ],
      injector,
    );
    injectors.push(checkInjector);

    return { storage, worker, port: worker.port, reload, oAuthService, checkInjector };
  }

  type Tab = ReturnType<typeof openTab>;

  async function openLoadedTab(options: { search?: string; session?: Map<string, string> } = {}) {
    const tab = openTab(options);
    tab.port.drain();
    await tab.storage.ready;
    return tab;
  }

  // Stores a token response like the OAuth library does: key by key in one synchronous run.
  async function signIn(tab: Tab, accessToken: string) {
    tab.storage.setItem('access_token', accessToken);
    tab.storage.setItem('expires_at', String(Date.now() + 3_600_000));
    tab.storage.setItem('access_token_stored_at', String(Date.now()));
    await Promise.resolve();
    tab.port.drain();
  }

  function deliver(...tabs: Tab[]) {
    tabs.forEach(tab => tab.port.drain());
  }

  beforeEach(() => {
    vi.useFakeTimers();
    FakeSharedWorker.scope = null;
    (globalThis as any).SharedWorker = FakeSharedWorker;
    sharedLocal = new Map();
  });

  afterEach(() => {
    injectors.reverse().forEach(injector => injector.destroy());
    injectors.length = 0;
    TestBed.resetTestingModule();
    delete (globalThis as any).SharedWorker;
    vi.useRealTimers();
  });

  it('should give a duplicated tab the tokens before it initializes', async () => {
    const a = await openLoadedTab();
    await signIn(a, 'at-1');

    const b = openTab();
    let ready = false;
    b.storage.ready.then(() => (ready = true));
    await Promise.resolve();
    expect(ready).toBe(false);

    b.port.drain();
    await b.storage.ready;

    expect(b.oAuthService.hasValidAccessToken()).toBe(true);
    expect(b.reload).not.toHaveBeenCalled();
  });

  it('should keep the tokens of the other tabs when a sign-in overtakes the configuration', async () => {
    const a = await openLoadedTab();
    const b = await openLoadedTab();

    // The first tab sent the configuration request without a token, then the second tab signs in.
    await signIn(b, 'at-2');
    deliver(a);
    checkAccessToken(a.checkInjector);
    deliver(a, b);
    vi.advanceTimersByTime(200);

    expect(b.storage.getItem('access_token')).toBe('at-2');
    expect(b.reload).not.toHaveBeenCalled();
    expect(a.reload).toHaveBeenCalled();
  });

  it('should keep the tokens of the other tabs when a signing-in tab gets them', async () => {
    const a = await openLoadedTab();
    const b = await openLoadedTab();

    // The first tab started a sign-in for a 401 while its configuration request is pending.
    a.storage.markSignInStarted();
    await signIn(b, 'at-2');
    deliver(a);
    checkAccessToken(a.checkInjector);
    deliver(a, b);
    vi.advanceTimersByTime(200);

    expect(b.storage.getItem('access_token')).toBe('at-2');
    expect(b.reload).not.toHaveBeenCalled();
  });

  it('should keep the tokens of the other tabs when the sign-in callback of a tab fails', async () => {
    const a = await openLoadedTab({ search: '?code=expired&state=x' });
    const b = await openLoadedTab();

    // The code exchange of the first tab fails, so it never stores its own token.
    await signIn(b, 'at-2');
    deliver(a);
    checkAccessToken(a.checkInjector);
    deliver(a, b);

    expect(b.storage.getItem('access_token')).toBe('at-2');
    expect(a.reload).toHaveBeenCalledTimes(1);
  });

  it('should keep the tokens when a refreshed token overtakes an anonymous configuration', async () => {
    const a = await openLoadedTab();
    await signIn(a, 'at-1');
    const b = await openLoadedTab();

    // The configuration of the second tab was loaded with an expired token, then the first tab
    // refreshed the tokens.
    await signIn(a, 'at-2');
    deliver(b);
    checkAccessToken(b.checkInjector);
    deliver(a, b);
    vi.advanceTimersByTime(200);

    expect(a.storage.getItem('access_token')).toBe('at-2');
    expect(a.reload).not.toHaveBeenCalled();
  });

  it('should keep the shared tokens when they are late again after a reload', async () => {
    const a = await openLoadedTab();
    await signIn(a, 'at-1');
    const session = new Map([['abp-late-token-reload', 'true']]);
    const b = openTab({ session });
    vi.advanceTimersByTime(3000);
    await b.storage.ready;

    // The worker answers after the timeout again; the tab already used its reload.
    b.port.drain();
    checkAccessToken(b.checkInjector);
    deliver(a, b);
    vi.advanceTimersByTime(200);

    expect(a.storage.getItem('access_token')).toBe('at-1');
    expect(a.reload).not.toHaveBeenCalled();
    expect(b.reload).not.toHaveBeenCalled();
  });

  it('should clear the tokens of a tab whose own token gets an anonymous configuration', async () => {
    const a = openTab();
    a.worker.listeners['error']();
    await a.storage.ready;
    await signIn(a, 'at-1');

    checkAccessToken(a.checkInjector);

    expect(a.storage.getItem('access_token')).toBeNull();
  });

  it('should send a token response to the worker at once', async () => {
    const a = await openLoadedTab();
    a.port.toWorker = [];

    a.storage.setItem('access_token', 'at-1');
    a.storage.setItem('expires_at', '123');
    await Promise.resolve();

    expect(a.port.toWorker).toEqual([
      {
        action: 'setMany',
        items: [
          ['access_token', 'at-1'],
          ['expires_at', '123'],
        ],
      },
    ]);
    // A tab that loads now reads both values, never the new token with an old expiration.
    a.port.drain();
    const b = await openLoadedTab();
    expect(b.storage.getItem('access_token')).toBe('at-1');
    expect(b.storage.getItem('expires_at')).toBe('123');
  });

  it('should sign out every tab when one tab signs out', async () => {
    const a = await openLoadedTab();
    await signIn(a, 'at-1');
    const b = await openLoadedTab();

    clearOAuthStorage(a.checkInjector);
    deliver(a, b);
    vi.advanceTimersByTime(200);

    expect(b.storage.getItem('access_token')).toBeNull();
    expect(b.reload).toHaveBeenCalled();
  });
});
