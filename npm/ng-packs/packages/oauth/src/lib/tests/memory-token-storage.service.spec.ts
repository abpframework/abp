import { DOCUMENT } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { AbpLocalStorageService } from '@abp/ng.core';
import { MemoryTokenStorageService } from '../services/memory-token-storage.service';

class FakePort {
  messages: any[] = [];
  onmessage: ((event: { data: any }) => void) | null = null;
  start() {}
  postMessage(message: any) {
    this.messages.push(message);
  }
  receive(data: any) {
    this.onmessage?.({ data });
  }
}

class FakeSharedWorker {
  static last: FakeSharedWorker;
  port = new FakePort();
  listeners: Record<string, () => void> = {};
  constructor() {
    FakeSharedWorker.last = this;
  }
  addEventListener(type: string, listener: () => void) {
    this.listeners[type] = listener;
  }
}

describe('MemoryTokenStorageService', () => {
  let reload: ReturnType<typeof vi.fn>;
  let session: Map<string, string>;
  let localStorageService: { getItem: any; setItem: any; removeItem: any };

  function create(options: { search?: string; hash?: string } = {}) {
    reload = vi.fn();
    session = new Map();
    const defaultView = {
      location: { search: options.search ?? '', hash: options.hash ?? '', reload },
      sessionStorage: {
        getItem: (key: string) => session.get(key) ?? null,
        setItem: (key: string, value: string) => session.set(key, value),
        removeItem: (key: string) => session.delete(key),
      },
      addEventListener: vi.fn(),
      removeEventListener: vi.fn(),
    };
    localStorageService = { getItem: vi.fn(() => null), setItem: vi.fn(), removeItem: vi.fn() };

    TestBed.configureTestingModule({
      providers: [
        { provide: DOCUMENT, useValue: { defaultView } },
        { provide: AbpLocalStorageService, useValue: localStorageService },
      ],
    });
    return TestBed.inject(MemoryTokenStorageService);
  }

  const port = () => FakeSharedWorker.last.port;

  function answerSnapshot(tokens: Record<string, string> = {}) {
    const request = port().messages.find(m => m.action === 'snapshot');
    port().receive({ action: 'snapshot', id: request.id, tokens });
  }

  beforeEach(() => {
    (globalThis as any).SharedWorker = FakeSharedWorker;
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
    delete (globalThis as any).SharedWorker;
    TestBed.resetTestingModule();
  });

  it('should complete ready after the snapshot of the worker', async () => {
    const storage = create();
    let ready = false;
    storage.ready.then(() => (ready = true));

    await Promise.resolve();
    expect(ready).toBe(false);

    answerSnapshot({ access_token: 'token', expires_at: '123' });
    await storage.ready;

    expect(storage.getItem('access_token')).toBe('token');
    expect(storage.getItem('expires_at')).toBe('123');
    expect(storage.timedOut).toBe(false);
  });

  it('should ignore a snapshot that it did not ask for', async () => {
    const storage = create();

    port().receive({ action: 'snapshot', id: 'other', tokens: { access_token: 'token' } });

    expect(storage.getItem('access_token')).toBeNull();
  });

  it('should stop waiting after the timeout', async () => {
    const storage = create();

    vi.advanceTimersByTime(3000);
    await storage.ready;

    expect(storage.timedOut).toBe(true);
  });

  it('should stop waiting when the worker fails', async () => {
    const storage = create();

    FakeSharedWorker.last.listeners['error']();
    await storage.ready;

    expect(storage.timedOut).toBe(true);
  });

  it('should keep a newer value from a broadcast over the snapshot', async () => {
    const storage = create();

    port().receive({ action: 'set', key: 'access_token', value: 'new' });
    answerSnapshot({ access_token: 'old', expires_at: '123' });
    await storage.ready;

    expect(storage.getItem('access_token')).toBe('new');
    expect(storage.getItem('expires_at')).toBe('123');
  });

  it('should not reload while loading the tokens when another tab gets a token', () => {
    const storage = create();

    port().receive({ action: 'set', key: 'access_token', value: 'token' });
    vi.advanceTimersByTime(200);

    expect(reload).not.toHaveBeenCalled();
    expect(storage.getItem('access_token')).toBe('token');
  });

  it('should reload an anonymous tab when another tab signs in', async () => {
    const storage = create();
    answerSnapshot();
    await storage.ready;

    port().receive({ action: 'set', key: 'access_token', value: 'token' });
    vi.advanceTimersByTime(200);

    expect(reload).toHaveBeenCalledTimes(1);
    expect(storage.handleTokensFromOtherTab()).toBe(true);
  });

  it('should apply the token response of another tab at once', async () => {
    const storage = create();
    answerSnapshot();
    await storage.ready;

    port().receive({
      action: 'setMany',
      items: [
        ['access_token', 'token'],
        ['expires_at', '123'],
      ],
    });

    expect(storage.getItem('access_token')).toBe('token');
    expect(storage.getItem('expires_at')).toBe('123');
  });

  it('should not reload a tab that handles a sign-in callback in the fragment', async () => {
    const storage = create({ hash: '#code=abc&state=xyz' });
    answerSnapshot();
    await storage.ready;

    port().receive({ action: 'set', key: 'access_token', value: 'token' });
    vi.advanceTimersByTime(200);

    expect(reload).not.toHaveBeenCalled();
  });

  it('should not reload a tab that handles its own sign-in callback', async () => {
    const storage = create({ search: '?code=abc&state=xyz' });
    answerSnapshot();
    await storage.ready;

    port().receive({ action: 'set', key: 'access_token', value: 'token' });
    vi.advanceTimersByTime(200);

    expect(reload).not.toHaveBeenCalled();
  });

  it('should record a token from another tab while signing in, without a reload', async () => {
    const storage = create();
    answerSnapshot();
    await storage.ready;

    storage.markSignInStarted();
    port().receive({ action: 'set', key: 'access_token', value: 'token' });
    vi.advanceTimersByTime(200);

    expect(reload).not.toHaveBeenCalled();
    expect(storage.handleTokensFromOtherTab()).toBe(true);
    expect(reload).toHaveBeenCalledTimes(1);
  });

  it('should treat a token refreshed by another tab as newer than the configuration', async () => {
    const storage = create();
    answerSnapshot({ access_token: 'old' });
    await storage.ready;

    port().receive({ action: 'set', key: 'access_token', value: 'new' });
    vi.advanceTimersByTime(200);

    // The tab was signed in already, so it isn't reloaded for the new token.
    expect(reload).not.toHaveBeenCalled();
    expect(storage.handleTokensFromOtherTab()).toBe(true);
  });

  it('should reload once when the tokens arrive after the timeout', async () => {
    const storage = create();
    vi.advanceTimersByTime(3000);
    await storage.ready;

    storage.setItem('abpOAuthClientId', 'client');
    answerSnapshot({ access_token: 'token', abpOAuthClientId: 'client' });

    expect(storage.getItem('access_token')).toBe('token');
    expect(reload).toHaveBeenCalledTimes(1);
    expect(session.get('abp-late-token-reload')).toBe('true');
  });

  it('should keep the shared tokens when they are late again after a reload', async () => {
    const storage = create();
    session.set('abp-late-token-reload', 'true');
    vi.advanceTimersByTime(3000);
    await storage.ready;

    answerSnapshot({ access_token: 'token' });

    expect(reload).not.toHaveBeenCalled();
    // The tokens came from another tab, so they aren't cleared for this tab's configuration.
    expect(storage.handleTokensFromOtherTab()).toBe(true);
    expect(reload).not.toHaveBeenCalled();
  });

  it('should not reload again for a token broadcast after the timeout', async () => {
    const storage = create();
    session.set('abp-late-token-reload', 'true');
    vi.advanceTimersByTime(3000);
    await storage.ready;

    port().receive({ action: 'setMany', items: [['access_token', 'token']] });
    vi.advanceTimersByTime(200);

    expect(reload).not.toHaveBeenCalled();
    expect(storage.getItem('access_token')).toBe('token');
  });

  it('should ignore late tokens when the tab got its own token', async () => {
    const storage = create();
    vi.advanceTimersByTime(3000);
    await storage.ready;

    storage.setItem('access_token', 'own');
    answerSnapshot({ access_token: 'other' });

    expect(storage.getItem('access_token')).toBe('own');
    expect(reload).not.toHaveBeenCalled();
  });

  it('should not treat the tokens loaded at startup as tokens from another tab', async () => {
    const storage = create();
    answerSnapshot({ access_token: 'token' });
    await storage.ready;

    expect(storage.handleTokensFromOtherTab()).toBe(false);
  });

  it('should not treat its own token as a token from another tab', async () => {
    const storage = create();
    FakeSharedWorker.last.listeners['error']();
    await storage.ready;

    storage.setItem('access_token', 'own');

    expect(storage.handleTokensFromOtherTab()).toBe(false);
  });

  it('should share a stored token with the other tabs', async () => {
    const storage = create();
    answerSnapshot();
    await storage.ready;

    storage.setItem('access_token', 'token');
    storage.setItem('expires_at', '123');
    await Promise.resolve();

    expect(port().messages).toContainEqual({
      action: 'setMany',
      items: [
        ['access_token', 'token'],
        ['expires_at', '123'],
      ],
    });
  });

  it('should keep the sign-in values of the tab in sessionStorage', () => {
    const storage = create();

    storage.setItem('nonce', 'value');

    expect(session.get('nonce')).toBe('value');
    expect(localStorageService.setItem).not.toHaveBeenCalled();
  });

  it('should keep the refresh token in localStorage', () => {
    const storage = create();

    storage.setItem('refresh_token', 'token');

    expect(localStorageService.setItem).toHaveBeenCalledWith('refresh_token', 'token');
  });

  it('should complete ready right away without a SharedWorker', async () => {
    delete (globalThis as any).SharedWorker;
    const storage = create();

    await storage.ready;

    expect(storage.timedOut).toBe(false);
  });
});
