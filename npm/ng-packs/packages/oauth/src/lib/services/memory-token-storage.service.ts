import { DestroyRef, DOCUMENT, inject, Injectable } from '@angular/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { AbpLocalStorageService } from '@abp/ng.core';
import { fromEvent } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

const LATE_TOKEN_RELOAD_KEY = 'abp-late-token-reload';
const TOKEN_LOAD_TIMEOUT = 3000;

@Injectable({
  providedIn: 'root',
})
export class MemoryTokenStorageService implements OAuthStorage {
  private static workerUrl: string | null = null;

  private keysShouldStoreInMemory = [
    'access_token',
    'id_token',
    'expires_at',
    'id_token_claims_obj',
    'id_token_expires_at',
    'id_token_stored_at',
    'access_token_stored_at',
    'abpOAuthClientId',
    'granted_scopes',
  ];

  private keysShouldStoreInSession = ['nonce', 'PKCE_verifier', 'session_state'];

  private worker?: any;
  private port?: MessagePort;
  private cache = new Map<string, string>();
  private localStorageService = inject(AbpLocalStorageService);
  private _document = inject(DOCUMENT);
  private destroyRef = inject(DestroyRef);
  private useSharedWorker = false;

  /**
   * Completes when this tab has the tokens of the other tabs, or stopped waiting for them.
   */
  readonly ready: Promise<void>;

  /**
   * True when the tab stopped waiting before it got the tokens (timeout or worker error).
   */
  timedOut = false;

  private resolveReady!: () => void;
  private loadingTokens = true;
  private snapshotRequestId?: string;
  private hasOwnToken = false;
  // True when this tab got an access token from another tab after it loaded the tokens, so its
  // application configuration can be older than its token.
  private otherTabTokens = false;
  private reloading = false;
  private pendingWrites = new Map<string, string>();

  // True from the start of a sign-in in this tab until it stores its own token. The OAuth library
  // removes the code from the URL before it exchanges it, so the URL can't be checked later.
  private signingIn = false;

  constructor() {
    this.ready = new Promise(resolve => (this.resolveReady = resolve));
    // The OAuth library reads the code from the query, or from the fragment when the query is empty.
    const location = this._document.defaultView?.location;
    this.signingIn = /[?&#]code=/.test(`${location?.search ?? ''}${location?.hash ?? ''}`);
    this.initializeStorage();
    this.setupCleanup();
  }

  /**
   * True when this tab stored tokens of its own, from its sign-in or refresh.
   */
  get hasOwnTokens(): boolean {
    return this.hasOwnToken;
  }

  /**
   * Called when this tab starts a sign-in, so a token from another tab doesn't reload it.
   */
  markSignInStarted(): void {
    this.signingIn = true;
  }

  markSignInFinished(): void {
    this.signingIn = false;
  }

  /**
   * Handles a valid token with an anonymous application configuration when the token came from
   * another tab after this tab loaded. Only this tab is behind then: it is reloaded once, and the
   * shared tokens are kept. Returns false for other mismatches.
   */
  handleTokensFromOtherTab(): boolean {
    if (!this.otherTabTokens) {
      return false;
    }

    if (!this.reloading) {
      this.reloadOnce();
    }
    return true;
  }

  private initializeStorage(): void {
    // @ts-ignore
    if (typeof SharedWorker !== 'undefined') {
      try {
        // Create worker from data URL to avoid path resolution issues in consuming apps
        // Data URLs are deterministic - same content produces same URL across all tabs
        if (!MemoryTokenStorageService.workerUrl) {
          MemoryTokenStorageService.workerUrl = this.createWorkerDataUrl();
        }
        // @ts-ignore
        this.worker = new SharedWorker(MemoryTokenStorageService.workerUrl, {
          name: 'oauth-token-storage',
        });
        this.port = this.worker.port;
        this.port.start();
        this.useSharedWorker = true;

        this.port.onmessage = event => {
          const { action, key, value } = event.data;

          switch (action) {
            case 'set':
              this.applyFromOtherTab(key, value);
              break;
            case 'setMany':
              (event.data.items as Array<[string, string]>).forEach(([itemKey, itemValue]) =>
                this.applyFromOtherTab(itemKey, itemValue),
              );
              break;
            case 'remove':
              this.cache.delete(key);
              this.refreshDocument();
              break;
            case 'clear':
              this.cache.clear();
              this.refreshDocument();
              break;
            case 'snapshot':
              if (event.data.id === this.snapshotRequestId) {
                this.handleInitialSnapshot(event.data.tokens);
              }
              break;
          }
        };

        // A data URL worker blocked by the CSP fails after the constructor returns.
        this.worker.addEventListener?.('error', () => {
          this.useSharedWorker = false;
          this.stopWaitingForTokens();
        });

        // Loads all tokens at once. A late answer is still used, see handleInitialSnapshot.
        this.snapshotRequestId = Math.random().toString(36).slice(2);
        this.port.postMessage({ action: 'snapshot', id: this.snapshotRequestId });
        setTimeout(() => this.stopWaitingForTokens(), TOKEN_LOAD_TIMEOUT);
        return;
      } catch (error) {
        this.useSharedWorker = false;
      }
    } else {
      this.useSharedWorker = false;
    }

    this.finishLoadingTokens();
  }

  private handleInitialSnapshot(tokens: Record<string, string>): void {
    const isLate = !this.loadingTokens && this.timedOut;
    // A tab that signed in on its own keeps its own tokens.
    if (isLate && (this.hasOwnToken || this.signingIn)) {
      return;
    }

    let applied = false;
    Object.keys(tokens).forEach(key => {
      // A broadcast or a value of this tab is newer than this snapshot.
      if (this.keysShouldStoreInMemory.includes(key) && !this.cache.has(key)) {
        this.cache.set(key, tokens[key]);
        applied ||= key === 'access_token';
      }
    });

    if (!isLate) {
      this.finishLoadingTokens();
    } else if (applied) {
      // The tab started without the tokens of the other tabs.
      this.otherTabTokens = true;
      this.reloadOnce();
    }
  }

  private finishLoadingTokens(): void {
    if (!this.loadingTokens) {
      return;
    }

    this.loadingTokens = false;
    if (!this.timedOut) {
      this.getSessionStorage()?.removeItem(LATE_TOKEN_RELOAD_KEY);
    }
    this.resolveReady();
  }

  private stopWaitingForTokens(): void {
    if (this.loadingTokens) {
      this.timedOut = true;
      this.finishLoadingTokens();
    }
  }

  // Reloads this tab at most once until the tokens load in time, so a slow worker can't loop.
  private reloadOnce(): void {
    const sessionStorage = this.getSessionStorage();
    if (!sessionStorage || sessionStorage.getItem(LATE_TOKEN_RELOAD_KEY)) {
      return;
    }

    sessionStorage.setItem(LATE_TOKEN_RELOAD_KEY, 'true');
    this.reloading = true;
    this._document.defaultView?.location.reload();
  }

  getItem(key: string): string | null {
    if (this.keysShouldStoreInMemory.includes(key)) {
      return this.cache.get(key) || null;
    }

    if (this.keysShouldStoreInSession.includes(key)) {
      return this.getSessionItem(key);
    }

    return this.localStorageService.getItem(key);
  }

  setItem(key: string, value: string): void {
    if (key === 'access_token') {
      this.hasOwnToken = true;
      this.otherTabTokens = false;
      this.signingIn = false;
    }

    if (this.keysShouldStoreInMemory.includes(key)) {
      this.cache.set(key, value);
      if (this.useSharedWorker && this.port) {
        this.queueWrite(key, value);
      }

      return;
    }

    if (this.keysShouldStoreInSession.includes(key)) {
      this.setSessionItem(key, value);
      return;
    }

    this.localStorageService.setItem(key, value);
  }

  removeItem(key: string): void {
    if (this.keysShouldStoreInMemory.includes(key)) {
      this.cache.delete(key);
      if (this.useSharedWorker && this.port) {
        this.flushWrites();
        this.port.postMessage({ action: 'remove', key });
      }

      return;
    }

    if (this.keysShouldStoreInSession.includes(key)) {
      this.removeSessionItem(key);
      return;
    }

    this.localStorageService.removeItem(key);
  }

  clear(): void {
    if (this.useSharedWorker && this.port) {
      this.pendingWrites.clear();
      this.port.postMessage({ action: 'clear' });
    }
    this.cache.clear();
  }

  private cleanupPort(): void {
    if (this.useSharedWorker && this.port) {
      try {
        this.flushWrites();
        this.port.postMessage({ action: 'disconnect' });
      } catch (error) {
        //
      }
    }
  }

  private setupCleanup(): void {
    if (this._document.defaultView) {
      fromEvent(this._document.defaultView, 'beforeunload')
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe(() => this.cleanupPort());

      fromEvent(this._document.defaultView, 'pagehide')
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe(() => this.cleanupPort());
    }
  }

  // The OAuth library stores a token response key by key in one synchronous run. They are sent
  // together, so a tab that starts meanwhile never reads a new access token with an old expiration.
  private queueWrite(key: string, value: string): void {
    if (!this.pendingWrites.size) {
      queueMicrotask(() => this.flushWrites());
    }
    this.pendingWrites.delete(key);
    this.pendingWrites.set(key, value);
  }

  private flushWrites(): void {
    if (!this.pendingWrites.size || !this.useSharedWorker || !this.port) {
      return;
    }

    const items = [...this.pendingWrites];
    this.pendingWrites.clear();
    this.port.postMessage({ action: 'setMany', items });
  }

  private applyFromOtherTab(key: string, value: string): void {
    this.checkAuthStateChanges(key, value);
    this.cache.set(key, value);
  }

  private checkAuthStateChanges(key: string, value: string): void {
    // A tab that is still loading the tokens gets them from the broadcast, so its configuration
    // request uses them.
    if (key !== 'access_token' || this.loadingTokens || this.cache.get(key) === value) {
      return;
    }

    // The configuration of this tab can be older than this token, from its sign-in or refresh.
    const hadToken = !!this.cache.get(key);
    this.otherTabTokens = true;
    // An anonymous tab reloads with the token, unless it is signing in on its own. A tab that
    // stopped waiting for a slow worker reloads only once, like for a late snapshot.
    if (!hadToken && !this.signingIn) {
      if (this.timedOut) {
        this.reloadOnce();
      } else {
        this.refreshDocument();
      }
    }
  }

  private refreshDocument(): void {
    this.reloading = true;
    this.cleanupPort();
    setTimeout(() => {
      this._document.defaultView?.location.reload();
    }, 100);
  }

  private getSessionStorage(): Storage | null {
    try {
      return this._document.defaultView?.sessionStorage ?? null;
    } catch {
      return null;
    }
  }

  private getSessionItem(key: string): string | null {
    const storage = this.getSessionStorage();
    return storage ? storage.getItem(key) : this.localStorageService.getItem(key);
  }

  private setSessionItem(key: string, value: string): void {
    const storage = this.getSessionStorage();
    if (storage) {
      storage.setItem(key, value);
    } else {
      this.localStorageService.setItem(key, value);
    }
  }

  private removeSessionItem(key: string): void {
    const storage = this.getSessionStorage();
    if (storage) {
      storage.removeItem(key);
    } else {
      this.localStorageService.removeItem(key);
    }
  }

  private createWorkerDataUrl(): string {
    const workerScript = `const tokenStore = new Map();
const ports = new Set();

function broadcastToOtherPorts(senderPort, message) {
  const deadPorts = [];
  ports.forEach(p => {
    if (p !== senderPort) {
      try {
        p.postMessage(message);
      } catch (error) {
        deadPorts.push(p);
      }
    }
  });
  deadPorts.forEach(p => ports.delete(p));
}

function removePort(port) {
  if (ports.has(port)) {
    ports.delete(port);
  }
}

self.onconnect = (event) => {
  const port = event.ports[0];
  ports.add(port);

  port.addEventListener('messageerror', () => {
    removePort(port);
  });

  port.onmessage = (e) => {
    const { action, key, value } = e.data;

    switch (action) {
      case 'set':
        if (key && value !== undefined) {
          tokenStore.set(key, value);
          broadcastToOtherPorts(port, { action: 'set', key, value });
        }
        break;

      case 'remove':
        if (key) {
          tokenStore.delete(key);
          broadcastToOtherPorts(port, { action: 'remove', key });
        }
        break;

      case 'clear':
        tokenStore.clear();
        broadcastToOtherPorts(port, { action: 'clear' });
        break;

      case 'setMany':
        (e.data.items || []).forEach(([itemKey, itemValue]) => tokenStore.set(itemKey, itemValue));
        broadcastToOtherPorts(port, { action: 'setMany', items: e.data.items || [] });
        break;

      case 'get':
        if (key) {
          const value = tokenStore.get(key) ?? null;
          port.postMessage({ action: 'get', key, value });
        }
        break;

      case 'snapshot':
        port.postMessage({ action: 'snapshot', id: e.data.id, tokens: Object.fromEntries(tokenStore) });
        break;

      case 'disconnect':
        removePort(port);
        break;

      default:
        //
    }
  };

  port.start();
};`;
    return 'data:application/javascript;base64,' + btoa(workerScript);
  }
}
