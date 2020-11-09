import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { of } from 'rxjs';
import { GetAppConfiguration } from '../actions';
import { SessionStateService } from '../services';
import * as AuthFlowStrategy from '../strategies/auth-flow.strategy';
import { CORE_OPTIONS } from '../tokens/options.token';
import { checkAccessToken, getInitialData, localeInitializer } from '../utils';
import * as environmentUtils from '../utils/environment-utils';
import * as multiTenancyUtils from '../utils/multi-tenancy-utils';

@Component({
  selector: 'abp-dummy',
  template: '',
})
export class DummyComponent {}

describe('InitialUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    mocks: [Store, OAuthService],
    providers: [
      {
        provide: CORE_OPTIONS,
        useValue: {
          environment: { oAuthConfig: { issuer: 'test' } },
          registerLocaleFn: () => Promise.resolve(),
        },
      },
    ],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getInitialData', () => {
    test('should dispatch GetAppConfiguration and return', async () => {
      const injector = spectator.inject(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const store = spectator.inject(Store);
      const dispatchSpy = jest.spyOn(store, 'dispatch');
      const parseTenantFromUrlSpy = jest.spyOn(multiTenancyUtils, 'parseTenantFromUrl');
      const getRemoteEnvSpy = jest.spyOn(environmentUtils, 'getRemoteEnv');
      parseTenantFromUrlSpy.mockReturnValue(Promise.resolve());
      getRemoteEnvSpy.mockReturnValue(Promise.resolve());

      injectorSpy.mockReturnValueOnce(store);
      injectorSpy.mockReturnValueOnce({ skipGetAppConfiguration: false });
      injectorSpy.mockReturnValueOnce({ init: () => null });
      injectorSpy.mockReturnValueOnce({ hasValidAccessToken: () => false });
      dispatchSpy.mockReturnValue(of('test'));

      expect(typeof getInitialData(injector)).toBe('function');
      expect(await getInitialData(injector)()).toBe('test');
      expect(dispatchSpy.mock.calls[0][0] instanceof GetAppConfiguration).toBeTruthy();
    });
  });

  describe('#checkAccessToken', () => {
    test('should call logOut fn of OAuthService when token is valid and current user not found', async () => {
      const injector = spectator.inject(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const clearOAuthStorageSpy = jest.spyOn(AuthFlowStrategy, 'clearOAuthStorage');

      injectorSpy.mockReturnValue({ hasValidAccessToken: () => true });

      checkAccessToken(
        {
          selectSnapshot: () => false,
        } as any,
        injector,
      );
      expect(clearOAuthStorageSpy).toHaveBeenCalled();
    });
  });

  describe('#localeInitializer', () => {
    test('should resolve registerLocale', async () => {
      const injector = spectator.inject(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const sessionState = spectator.inject(SessionStateService);
      injectorSpy.mockReturnValueOnce(sessionState);
      injectorSpy.mockReturnValueOnce({ registerLocaleFn: () => Promise.resolve() });
      expect(typeof localeInitializer(injector)).toBe('function');
      expect(await localeInitializer(injector)()).toBe('resolved');
    });
  });
});
