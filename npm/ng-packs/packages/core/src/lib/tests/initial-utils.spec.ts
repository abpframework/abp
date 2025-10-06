import { EnvironmentService } from '../services/environment.service';
import { AuthService } from '../abstracts/auth.service';
import { ConfigStateService } from '../services/config-state.service';
import { CORE_OPTIONS } from '../tokens/options.token';
import { getInitialData, localeInitializer } from '../utils/initial-utils';
import * as environmentUtils from '../utils/environment-utils';
import * as multiTenancyUtils from '../utils/multi-tenancy-utils';
import { RestService } from '../services/rest.service';
import { CHECK_AUTHENTICATION_STATE_FN_KEY } from '../tokens/check-authentication-state';
import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { AbpApplicationConfigurationService, SessionStateService } from '@abp/ng.core';
import { ApplicationConfigurationDto } from '@abp/ng.core';

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
    test('should be a function', () => {
      expect(typeof getInitialData).toBe('function');
    });
  });

  describe('#localeInitializer', () => {
    test('should be a function', () => {
      expect(typeof localeInitializer).toBe('function');
    });
  });
});
