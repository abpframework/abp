import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { ReplaySubject, timer, Subject, of } from 'rxjs';
import { Config } from '../models/config';
import { ApplicationConfigurationService, ConfigStateService } from '../services';
import { ConfigState } from '../states';
import { SetLanguage, PatchRouteByName } from '../actions';

export const CONFIG_STATE_DATA = {
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

describe('ConfigState', () => {
  let spectator: SpectatorService<ConfigStateService>;
  let store: SpyObject<Store>;
  let service: ConfigStateService;
  let state: ConfigState;
  let appConfigService: SpyObject<ApplicationConfigurationService>;

  const createService = createServiceFactory({
    service: ConfigStateService,
    mocks: [ApplicationConfigurationService, Store],
  });

  beforeEach(() => {
    spectator = createService();
    store = spectator.get(Store);
    service = spectator.service;
    appConfigService = spectator.get(ApplicationConfigurationService);
    state = new ConfigState(spectator.get(ApplicationConfigurationService), store);
  });

  describe('#getAll', () => {
    it('should return CONFIG_STATE_DATA', () => {
      expect(ConfigState.getAll(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA);
    });
  });

  describe('#getApplicationInfo', () => {
    it('should return application property', () => {
      expect(ConfigState.getApplicationInfo(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA.environment.application);
    });
  });

  describe('#getOne', () => {
    it('should return one property', () => {
      expect(ConfigState.getOne('environment')(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA.environment);
    });
  });

  describe('#getDeep', () => {
    it('should return deeper', () => {
      expect(ConfigState.getDeep('environment.localization.defaultResourceName')(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.environment.localization.defaultResourceName,
      );
      expect(ConfigState.getDeep(['environment', 'localization', 'defaultResourceName'])(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.environment.localization.defaultResourceName,
      );

      expect(ConfigState.getDeep('test')(null)).toBeFalsy();
    });
  });

  describe('#getRoute', () => {
    it('should return route', () => {
      expect(ConfigState.getRoute(null, '::Menu:Home')(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA.flattedRoutes[0]);
      expect(ConfigState.getRoute('identity')(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA.flattedRoutes[1]);
    });
  });

  describe('#getApiUrl', () => {
    it('should return api url', () => {
      expect(ConfigState.getApiUrl('other')(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA.environment.apis.other.url);
      expect(ConfigState.getApiUrl()(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA.environment.apis.default.url);
    });
  });

  describe('#getSetting', () => {
    it('should return a setting', () => {
      expect(ConfigState.getSetting('Abp.Localization.DefaultLanguage')(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.setting.values['Abp.Localization.DefaultLanguage'],
      );
    });
  });

  describe('#getSettings', () => {
    it('should return settings', () => {
      expect(ConfigState.getSettings('Localization')(CONFIG_STATE_DATA)).toEqual({
        'Abp.Localization.DefaultLanguage': 'en',
      });

      expect(ConfigState.getSettings('AllSettings')(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA.setting.values);
    });
  });

  describe('#getGrantedPolicy', () => {
    it('should return a granted policy', () => {
      expect(ConfigState.getGrantedPolicy('Abp.Identity')(CONFIG_STATE_DATA)).toBe(false);
      expect(ConfigState.getGrantedPolicy('')(CONFIG_STATE_DATA)).toBe(true);
    });
  });

  describe('#getLocalization', () => {
    it('should return a localization', () => {
      expect(ConfigState.getLocalization('AbpIdentity::Identity')(CONFIG_STATE_DATA)).toBe('identity');

      expect(ConfigState.getLocalization('AbpIdentity::NoIdentity')(CONFIG_STATE_DATA)).toBe('AbpIdentity::NoIdentity');

      expect(ConfigState.getLocalization({ key: '', defaultValue: 'default' })(CONFIG_STATE_DATA)).toBe('default');

      expect(ConfigState.getLocalization("::'{0}' and '{1}' do not match.", 'first', 'second')(CONFIG_STATE_DATA)).toBe(
        'first and second do not match.',
      );

      try {
        ConfigState.getLocalization('::Test')({
          ...CONFIG_STATE_DATA,
          environment: { ...CONFIG_STATE_DATA.environment, localization: {} as any },
        });
        expect(false).toBeTruthy(); // fail
      } catch (error) {
        expect((error as Error).message).toContain('Please check your environment');
      }
    });
  });

  describe('#GetAppConfiguration', () => {
    it('should call the getConfiguration of ApplicationConfigurationService and patch the state', done => {
      let patchStateArg;
      let dispatchArg;

      const configuration = {
        setting: { values: { 'Abp.Localization.DefaultLanguage': 'tr;TR' } },
      };

      const res$ = new ReplaySubject(1);
      res$.next(configuration);

      const patchState = jest.fn(s => (patchStateArg = s));
      const dispatch = jest.fn(a => {
        dispatchArg = a;
        return of(a);
      });
      appConfigService.getConfiguration.andReturn(res$);

      state.addData({ patchState, dispatch } as any).subscribe();

      timer(0).subscribe(() => {
        expect(patchStateArg).toEqual(configuration);
        expect(dispatchArg instanceof SetLanguage).toBeTruthy();
        expect(dispatchArg).toEqual({ payload: 'tr' });
        done();
      });
    });
  });

  describe('#PatchRouteByName', () => {
    it('should should patch the route', () => {
      let patchStateArg;

      const patchState = jest.fn(s => (patchStateArg = s));
      const getState = jest.fn(() => CONFIG_STATE_DATA);

      state.patchRoute(
        { patchState, getState } as any,
        new PatchRouteByName('::Menu:Home', {
          name: 'Home',
          path: 'home',
          children: [{ path: 'dashboard', name: 'Dashboard' }],
        }),
      );

      expect(patchStateArg.routes[0]).toEqual({
        name: 'Home',
        path: 'home',
        url: '/home',
        children: [{ path: 'dashboard', name: 'Dashboard', url: '/home/dashboard' }],
      });
    });

    it('should should patch the route without path', () => {
      let patchStateArg;

      const patchState = jest.fn(s => (patchStateArg = s));
      const getState = jest.fn(() => CONFIG_STATE_DATA);

      state.patchRoute(
        { patchState, getState } as any,
        new PatchRouteByName('::Menu:Home', {
          name: 'Main',
          children: [{ path: 'dashboard', name: 'Dashboard' }],
        }),
      );

      expect(patchStateArg.routes[0]).toEqual({
        name: 'Main',
        path: '',
        url: '/',
        children: [{ path: 'dashboard', name: 'Dashboard', url: '/dashboard' }],
      });
    });
  });
});
