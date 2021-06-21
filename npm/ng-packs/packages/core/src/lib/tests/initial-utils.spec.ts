import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { OAuthService } from 'angular-oauth2-oidc';
import { of } from 'rxjs';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { ApplicationConfigurationDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import {
  AuthService,
  ConfigStateService,
  EnvironmentService,
  SessionStateService,
} from '../services';
import * as AuthFlowStrategy from '../strategies/auth-flow.strategy';
import { CORE_OPTIONS } from '../tokens/options.token';
import { checkAccessToken, getInitialData, localeInitializer } from '../utils';
import * as environmentUtils from '../utils/environment-utils';
import * as multiTenancyUtils from '../utils/multi-tenancy-utils';

const environment = { oAuthConfig: { issuer: 'test' } };

@Component({
  selector: 'abp-dummy',
  template: '',
})
export class DummyComponent {}

describe('InitialUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    mocks: [
      EnvironmentService,
      ConfigStateService,
      AbpApplicationConfigurationService,
      AuthService,
      OAuthService,
      SessionStateService,
    ],
    providers: [
      {
        provide: CORE_OPTIONS,
        useValue: {
          environment,
          registerLocaleFn: () => Promise.resolve(),
        },
      },
    ],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getInitialData', () => {
    test('should call the getConfiguration method of ApplicationConfigurationService and set states', async () => {
      const environmentService = spectator.inject(EnvironmentService);
      const configStateService = spectator.inject(ConfigStateService);
      const sessionStateService = spectator.inject(SessionStateService);
      const applicationConfigurationService = spectator.inject(AbpApplicationConfigurationService);
      const parseTenantFromUrlSpy = jest.spyOn(multiTenancyUtils, 'parseTenantFromUrl');
      const getRemoteEnvSpy = jest.spyOn(environmentUtils, 'getRemoteEnv');
      parseTenantFromUrlSpy.mockReturnValue(Promise.resolve());
      getRemoteEnvSpy.mockReturnValue(Promise.resolve());

      const appConfigRes = {
        currentTenant: { id: 'test', name: 'testing' },
      } as ApplicationConfigurationDto;

      const getConfigurationSpy = jest.spyOn(applicationConfigurationService, 'get');
      getConfigurationSpy.mockReturnValue(of(appConfigRes));

      const environmentSetStateSpy = jest.spyOn(environmentService, 'setState');
      const configSetStateSpy = jest.spyOn(configStateService, 'setState');
      const sessionSetTenantSpy = jest.spyOn(sessionStateService, 'setTenant');

      const configStateGetOneSpy = jest.spyOn(configStateService, 'getOne');
      configStateGetOneSpy.mockReturnValue(appConfigRes.currentTenant);

      const mockInjector = {
        get: spectator.inject,
      };

      await getInitialData(mockInjector)();

      expect(typeof getInitialData(mockInjector)).toBe('function');
      expect(environmentSetStateSpy).toHaveBeenCalledWith(environment);
      expect(getConfigurationSpy).toHaveBeenCalled();
      expect(configSetStateSpy).toHaveBeenCalledWith(appConfigRes);
      expect(sessionSetTenantSpy).toHaveBeenCalledWith(appConfigRes.currentTenant);
    });
  });

  describe('#checkAccessToken', () => {
    test('should call logOut fn of OAuthService when token is valid and current user not found', async () => {
      const injector = spectator.inject(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const clearOAuthStorageSpy = jest.spyOn(AuthFlowStrategy, 'clearOAuthStorage');

      injectorSpy.mockReturnValueOnce({ getDeep: () => false });
      injectorSpy.mockReturnValueOnce({ hasValidAccessToken: () => true });

      checkAccessToken(injector);
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
