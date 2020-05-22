import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import clone from 'just-clone';
import { of, ReplaySubject, timer } from 'rxjs';
import { AddRoute, PatchRouteByName, SetLanguage } from '../actions';
import { ABP } from '../models';
import { Config } from '../models/config';
import { ApplicationConfigurationService, ConfigStateService } from '../services';
import { ConfigState } from '../states';
import { HttpClient } from '@angular/common/http';

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
          parentName: 'AbpAccount::Menu:Account',
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
          parentName: 'AbpAccount::Menu:Account',
        },
      ],
      url: '/account',
    },
    {
      path: 'login',
      name: 'AbpAccount::Login',
      order: 1,
      url: '/account/login',
      parentName: 'AbpAccount::Menu:Account',
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
    currentCulture: {
      displayName: 'English',
      englishName: 'English',
      threeLetterIsoLanguageName: 'eng',
      twoLetterIsoLanguageName: 'en',
      isRightToLeft: false,
      cultureName: 'en',
      name: 'en',
      nativeName: 'English',
      dateTimeFormat: {
        calendarAlgorithmType: 'SolarCalendar',
        dateTimeFormatLong: 'dddd, MMMM d, yyyy',
        shortDatePattern: 'M/d/yyyy',
        fullDateTimePattern: 'dddd, MMMM d, yyyy h:mm:ss tt',
        dateSeparator: '/',
        shortTimePattern: 'h:mm tt',
        longTimePattern: 'h:mm:ss tt',
      },
    },
    defaultResourceName: null,
  },
  auth: {
    policies: {
      'AbpIdentity.Roles': true,
    },
    grantedPolicies: {
      'Abp.Identity': false,
      'Abp.Account': true,
    },
  },
  setting: {
    values: {
      'Abp.Custom.SomeSetting': 'X',
      'Abp.Localization.DefaultLanguage': 'en',
    },
  },
  currentUser: {
    isAuthenticated: false,
    id: null,
    tenantId: null,
    userName: null,
    email: null,
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

  const createService = createServiceFactory({
    service: ConfigStateService,
    mocks: [ApplicationConfigurationService, Store, HttpClient],
  });

  beforeEach(() => {
    spectator = createService();
    store = spectator.get(Store);
    service = spectator.service;
    state = new ConfigState(spectator.get(HttpClient), store);
  });

  describe('#getAll', () => {
    it('should return CONFIG_STATE_DATA', () => {
      expect(ConfigState.getAll(CONFIG_STATE_DATA)).toEqual(CONFIG_STATE_DATA);
    });
  });

  describe('#getApplicationInfo', () => {
    it('should return application property', () => {
      expect(ConfigState.getApplicationInfo(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.environment.application,
      );
    });
  });

  describe('#getOne', () => {
    it('should return one property', () => {
      expect(ConfigState.getOne('environment')(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.environment,
      );
    });
  });

  describe('#getDeep', () => {
    it('should return deeper', () => {
      expect(
        ConfigState.getDeep('environment.localization.defaultResourceName')(CONFIG_STATE_DATA),
      ).toEqual(CONFIG_STATE_DATA.environment.localization.defaultResourceName);
      expect(
        ConfigState.getDeep(['environment', 'localization', 'defaultResourceName'])(
          CONFIG_STATE_DATA,
        ),
      ).toEqual(CONFIG_STATE_DATA.environment.localization.defaultResourceName);

      expect(ConfigState.getDeep('test')(null)).toBeFalsy();
    });
  });

  describe('#getRoute', () => {
    it('should return route', () => {
      expect(ConfigState.getRoute(null, '::Menu:Home')(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.flattedRoutes[0],
      );
      expect(ConfigState.getRoute('account')(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.flattedRoutes[1],
      );
    });
  });

  describe('#getApiUrl', () => {
    it('should return api url', () => {
      expect(ConfigState.getApiUrl('other')(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.environment.apis.other.url,
      );
      expect(ConfigState.getApiUrl()(CONFIG_STATE_DATA)).toEqual(
        CONFIG_STATE_DATA.environment.apis.default.url,
      );
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
    test.each`
      keyword           | expected
      ${undefined}      | ${CONFIG_STATE_DATA.setting.values}
      ${'Localization'} | ${{ 'Abp.Localization.DefaultLanguage': 'en' }}
      ${'X'}            | ${{}}
      ${'localization'} | ${{}}
    `('should return $expected when keyword is given as $keyword', ({ keyword, expected }) => {
      expect(ConfigState.getSettings(keyword)(CONFIG_STATE_DATA)).toEqual(expected);
    });
  });

  describe('#getGrantedPolicy', () => {
    it('should return a granted policy', () => {
      expect(ConfigState.getGrantedPolicy('Abp.Identity')(CONFIG_STATE_DATA)).toBe(false);
      expect(ConfigState.getGrantedPolicy('Abp.Identity || Abp.Account')(CONFIG_STATE_DATA)).toBe(
        true,
      );
      expect(ConfigState.getGrantedPolicy('Abp.Account && Abp.Identity')(CONFIG_STATE_DATA)).toBe(
        false,
      );
      expect(ConfigState.getGrantedPolicy('Abp.Account &&')(CONFIG_STATE_DATA)).toBe(false);
      expect(ConfigState.getGrantedPolicy('|| Abp.Account')(CONFIG_STATE_DATA)).toBe(false);
      expect(ConfigState.getGrantedPolicy('')(CONFIG_STATE_DATA)).toBe(true);
    });
  });

  describe('#getLocalization', () => {
    it('should return a localization', () => {
      expect(ConfigState.getLocalization('AbpIdentity::Identity')(CONFIG_STATE_DATA)).toBe(
        'identity',
      );

      expect(ConfigState.getLocalization('AbpIdentity::NoIdentity')(CONFIG_STATE_DATA)).toBe(
        'NoIdentity',
      );

      expect(
        ConfigState.getLocalization({ key: '', defaultValue: 'default' })(CONFIG_STATE_DATA),
      ).toBe('default');

      expect(
        ConfigState.getLocalization(
          "::'{0}' and '{1}' do not match.",
          'first',
          'second',
        )(CONFIG_STATE_DATA),
      ).toBe('first and second do not match.');

      expect(
        ConfigState.getLocalization('::Test')({
          ...CONFIG_STATE_DATA,
          environment: {
            ...CONFIG_STATE_DATA.environment,
            localization: {} as any,
          },
        }),
      ).toBe('Test');
    });
  });

  describe('#GetAppConfiguration', () => {
    it('should call the app-configuration API and patch the state', done => {
      let patchStateArg;
      let dispatchArg;

      const configuration = {
        setting: { values: { 'Abp.Localization.DefaultLanguage': 'tr;TR' } },
        localization: { currentCulture: {} },
      };

      const res$ = new ReplaySubject(1);
      res$.next(configuration);

      const patchState = jest.fn(s => (patchStateArg = s));
      const dispatch = jest.fn(a => {
        dispatchArg = a;
        return of(a);
      });
      const httpClient = spectator.get(HttpClient);
      httpClient.get.andReturn(res$);

      state.addData({ patchState, dispatch } as any).subscribe();

      timer(0).subscribe(() => {
        expect(patchStateArg).toEqual(configuration);
        expect(dispatchArg instanceof SetLanguage).toBeTruthy();
        expect(dispatchArg).toEqual({ payload: 'tr', dispatchAppConfiguration: false });
        done();
      });
    });
  });

  describe('#PatchRouteByName', () => {
    it('should patch the route', () => {
      let patchStateArg;

      const patchState = jest.fn(s => (patchStateArg = s));
      const getState = jest.fn(() => clone(CONFIG_STATE_DATA));

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
      expect(patchStateArg.flattedRoutes[0]).toEqual({
        name: 'Home',
        path: 'home',
        url: '/home',
        children: [{ path: 'dashboard', name: 'Dashboard', url: '/home/dashboard' }],
      });
    });

    it('should patch the route without path', () => {
      let patchStateArg;

      const patchState = jest.fn(s => (patchStateArg = s));
      const getState = jest.fn(() => clone(CONFIG_STATE_DATA));

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

      expect(patchStateArg.flattedRoutes[0]).toEqual({
        name: 'Main',
        path: '',
        url: '/',
        children: [{ path: 'dashboard', name: 'Dashboard', url: '/dashboard' }],
      });
    });
  });

  describe('#AddRoute', () => {
    const newRoute = {
      name: 'My new page',
      children: [],
      iconClass: 'fa fa-dashboard',
      path: 'page',
      invisible: false,
      order: 2,
      requiredPolicy: 'MyProjectName::MyNewPage',
    } as Omit<ABP.Route, 'children'>;

    test('should add a new route', () => {
      let patchStateArg;

      const patchState = jest.fn(s => (patchStateArg = s));
      const getState = jest.fn(() => clone(CONFIG_STATE_DATA));

      state.addRoute({ patchState, getState } as any, new AddRoute(newRoute));

      expect(patchStateArg.routes[CONFIG_STATE_DATA.routes.length]).toEqual({
        ...newRoute,
        url: '/page',
      });
      expect(patchStateArg.flattedRoutes[CONFIG_STATE_DATA.flattedRoutes.length]).toEqual(
        patchStateArg.routes[CONFIG_STATE_DATA.routes.length],
      );
    });

    it('should add a new child route', () => {
      let patchStateArg;

      const patchState = jest.fn(s => (patchStateArg = s));
      const getState = jest.fn(() => clone(CONFIG_STATE_DATA));

      state.addRoute(
        { patchState, getState } as any,
        new AddRoute({ ...newRoute, parentName: 'AbpAccount::Login' }),
      );

      expect(patchStateArg.routes[1].children[0].children[0]).toEqual({
        ...newRoute,
        parentName: 'AbpAccount::Login',
        url: '/account/login/page',
      });

      expect(patchStateArg.flattedRoutes[CONFIG_STATE_DATA.flattedRoutes.length]).toEqual(
        patchStateArg.routes[1].children[0].children[0],
      );

      expect(
        patchStateArg.flattedRoutes[
          CONFIG_STATE_DATA.flattedRoutes.findIndex(route => route.name === 'AbpAccount::Login')
        ],
      ).toEqual(patchStateArg.routes[1].children[0]);
    });
  });
});
