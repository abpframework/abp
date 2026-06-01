import { createComponentFactory, Spectator } from '@ngneat/spectator/vitest';
import { Component } from '@angular/core';
import { EnvironmentService } from '../services/environment.service';
import {SessionStateService} from '../services/session-state.service';
import { ConfigStateService } from '../services/config-state.service';
import { AuthService } from '../abstracts/auth.service';
import { CORE_OPTIONS } from '../tokens/options.token';
import { getInitialData, localeInitializer } from '../utils/initial-utils';
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
