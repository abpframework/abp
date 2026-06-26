import { DestroyRef, DOCUMENT, inject, Injectable } from '@angular/core';
import { OAuthStorage } from 'angular-oauth2-oidc';
import { AbpLocalStorageService } from '@abp/ng.core';
import { fromEvent } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

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

  constructor() {
    this.initializeStorage();
    this.setupCleanup();
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
              this.checkAuthStateChanges(key);
              this.cache.set(key, value);
              break;
            case 'remove':
              this.cache.delete(key);
              this.refreshDocument();
              break;
            case 'clear':
              this.cache.clear();
              this.refreshDocument();
              break;
            case 'get':
              if (value !== null) {
                this.cache.set(key, value);
              }
              break;
          }
        };

        // Load all tokens from SharedWorker on initialization
        this.keysShouldStoreInMemory.forEach(key => {
          this.port?.postMessage({ action: 'get', key });
        });
      } catch (error) {
        this.useSharedWorker = false;
      }
    } else {
      this.useSharedWorker = false;
    }
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
    if (this.keysShouldStoreInMemory.includes(key)) {
      if (this.useSharedWorker && this.port) {
        this.cache.set(key, value);
        this.port.postMessage({ action: 'set', key, value });
      } else {
        this.cache.set(key, value);
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
      if (this.useSharedWorker && this.port) {
        this.cache.delete(key);
        this.port.postMessage({ action: 'remove', key });
      } else {
        this.cache.delete(key);
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
      this.port.postMessage({ action: 'clear' });
    }
    this.cache.clear();
  }

  private cleanupPort(): void {
    if (this.useSharedWorker && this.port) {
      try {
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

  private checkAuthStateChanges = (key: string) => {
    if (key === 'access_token' && !this.cache.get('access_token')) {
      this.refreshDocument();
    }
  };

  private refreshDocument(): void {
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
    return this.getSessionStorage()?.getItem(key) ?? null;
  }

  private setSessionItem(key: string, value: string): void {
    this.getSessionStorage()?.setItem(key, value);
  }

  private removeSessionItem(key: string): void {
    this.getSessionStorage()?.removeItem(key);
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

      case 'get':
        if (key) {
          const value = tokenStore.get(key) ?? null;
          port.postMessage({ action: 'get', key, value });
        }
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
