import { Injector } from '@angular/core';
import { ConfigStateService } from '@abp/ng.core';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import * as clearOAuthStorageModule from '../utils/clear-o-auth-storage';
import { checkAccessToken } from '../utils/check-access-token';
import { MemoryTokenStorageService } from '../services/memory-token-storage.service';

describe('checkAccessToken', () => {
  let clearSpy: ReturnType<typeof vi.spyOn>;

  function injectorWith(storage: unknown, expiration = Date.now() + 60_000): Injector {
    return Injector.create({
      providers: [
        { provide: ConfigStateService, useValue: { getDeep: () => null } },
        {
          provide: OAuthService,
          useValue: { hasValidAccessToken: () => true, getAccessTokenExpiration: () => expiration },
        },
        { provide: OAuthStorage, useValue: storage },
      ],
    });
  }

  function memoryStorage(handled: boolean) {
    const storage = Object.create(MemoryTokenStorageService.prototype);
    storage.handleTokensFromOtherTab = vi.fn(() => handled);
    return storage;
  }

  beforeEach(() => {
    clearSpy = vi.spyOn(clearOAuthStorageModule, 'clearOAuthStorage').mockImplementation(() => {});
  });

  afterEach(() => vi.restoreAllMocks());

  it('should not clear the shared storage when the token came from another tab', () => {
    const storage = memoryStorage(true);

    checkAccessToken(injectorWith(storage));

    expect(storage.handleTokensFromOtherTab).toHaveBeenCalled();
    expect(clearSpy).not.toHaveBeenCalled();
  });

  it('should clear the storage for other mismatches', () => {
    checkAccessToken(injectorWith(memoryStorage(false)));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });

  it('should keep a token that expired within the allowed clock skew', () => {
    // For example its refresh couldn't reach the server, so the configuration is anonymous.
    checkAccessToken(injectorWith(memoryStorage(false), Date.now() - 60_000));

    expect(clearSpy).not.toHaveBeenCalled();
  });

  it('should clear a token without an expiration as before', () => {
    checkAccessToken(injectorWith(memoryStorage(false), null as any));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });

  it('should clear the storage with other storages', () => {
    checkAccessToken(injectorWith({ getItem: vi.fn() }));

    expect(clearSpy).toHaveBeenCalledTimes(1);
  });
});
