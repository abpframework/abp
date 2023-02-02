import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { ApplicationConfigurationDto } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import { SessionStateService } from '../services/session-state.service';
import { EnvironmentService } from '../services/environment.service';
import { AuthService } from '../abstracts/auth.service';
import { ConfigStateService } from '../services/config-state.service';
import { CORE_OPTIONS } from '../tokens/options.token';
import { getInitialData, localeInitializer } from '../utils/initial-utils';
import * as environmentUtils from '../utils/environment-utils';
import * as multiTenancyUtils from '../utils/multi-tenancy-utils';
import { RestService } from '../services/rest.service';
import { CHECK_AUTHENTICATION_STATE_FN_KEY } from '../tokens/check-authentication-state';

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
      SessionStateService,
      RestService,
    ],
    providers: [
      {
        provide: CORE_OPTIONS,
        useValue: {
          environment,
          registerLocaleFn: () => Promise.resolve(),
        },
      },
      {
        provide: CHECK_AUTHENTICATION_STATE_FN_KEY,
        useValue: () => {},
      },
    ],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getInitialData', () => {
    test('should call the getConfiguration method of ApplicationConfigurationService and set states', async () => {
      const environmentService = spectator.inject(EnvironmentService);
      const configStateService = spectator.inject(ConfigStateService);
      const sessionStateService = spectator.inject(SessionStateService);
      //const checkAuthenticationState = spectator.inject(CHECK_AUTHENTICATION_STATE_FN_KEY);

      const authService = spectator.inject(AuthService);

      const parseTenantFromUrlSpy = jest.spyOn(multiTenancyUtils, 'parseTenantFromUrl');
      const getRemoteEnvSpy = jest.spyOn(environmentUtils, 'getRemoteEnv');
      parseTenantFromUrlSpy.mockReturnValue(Promise.resolve());
      getRemoteEnvSpy.mockReturnValue(Promise.resolve());

      const appConfigRes = {
        currentTenant: { id: 'test', name: 'testing' },
      } as ApplicationConfigurationDto;

      const environmentSetStateSpy = jest.spyOn(environmentService, 'setState');
      const configRefreshAppStateSpy = jest.spyOn(configStateService, 'refreshAppState');
      configRefreshAppStateSpy.mockReturnValue(of(appConfigRes));
      const sessionSetTenantSpy = jest.spyOn(sessionStateService, 'setTenant');
      const authServiceInitSpy = jest.spyOn(authService, 'init');
      const configStateGetOneSpy = jest.spyOn(configStateService, 'getOne');
      configStateGetOneSpy.mockReturnValue(appConfigRes.currentTenant);

      const mockInjector = {
        get: spectator.inject,
      };

      await getInitialData(mockInjector)();

      expect(typeof getInitialData(mockInjector)).toBe('function');
      expect(configRefreshAppStateSpy).toHaveBeenCalled();
      expect(environmentSetStateSpy).toHaveBeenCalledWith(environment);
      expect(sessionSetTenantSpy).toHaveBeenCalledWith(appConfigRes.currentTenant);
      expect(authServiceInitSpy).toHaveBeenCalled();
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
