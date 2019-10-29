import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { ConfigStateService } from '../services/config-state.service';
import { ConfigState } from '../states';
import { Store } from '@ngxs/store';
import { Config } from '../models/config';

const CONFIG_STATE_DATA = {
  environment: {
    production: false,
    application: {
      name: 'MyProjectName',
    },
    oAuthConfig: {
      issuer: 'https://localhost:44305',
    },
    apis: {
      default: {
        url: 'https://localhost:44305',
      },
      other: {
        url: 'https://localhost:44306',
      },
    },
    localization: {
      defaultResourceName: 'MyProjectName',
    },
  },
  requirements: {
    layouts: [null, null, null],
  },
  routes: [
    {
      name: '::Menu:Home',
      path: '',
      children: [],
      url: '/',
    },
    {
      name: 'AbpAccount::Menu:Account',
      path: 'account',
      invisible: true,
      layout: 'application',
      children: [
        {
          path: 'login',
          name: 'AbpAccount::Login',
          order: 1,
          url: '/account/login',
        },
      ],
      url: '/account',
    },
  ],
  flattedRoutes: [
    {
      name: '::Menu:Home',
      path: '',
      children: [],
      url: '/',
    },
    {
      name: '::Menu:Identity',
      path: 'identity',
      children: [],
      url: '/identity',
    },
  ],
  localization: {
    values: {
      MyProjectName: {
        "'{0}' and '{1}' do not match.": "'{0}' and '{1}' do not match.",
      },
      AbpIdentity: {
        Identity: 'identity',
      },
    },
    languages: [
      {
        cultureName: 'cs',
        uiCultureName: 'cs',
        displayName: 'Čeština',
        flagIcon: null,
      },
    ],
  },
  auth: {
    policies: {
      'AbpIdentity.Roles': true,
    },
    grantedPolicies: {
      'Abp.Identity': false,
    },
  },
  setting: {
    values: {
      'Abp.Localization.DefaultLanguage': 'en',
    },
  },
  currentUser: {
    isAuthenticated: false,
    id: null,
    tenantId: null,
    userName: null,
  },
  features: {
    values: {},
  },
} as Config.State;

describe('ConfigStateService', () => {
  let service: ConfigStateService;
  let spectator: SpectatorService<ConfigStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: ConfigStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
  });
  test('should have the all ConfigState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    ConfigState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = ConfigState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          ConfigState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(ConfigState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(ConfigState[fnName]);
        }
      });
  });
});
