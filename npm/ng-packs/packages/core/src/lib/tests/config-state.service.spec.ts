import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import * as ConfigActions from '../actions';
import { ApplicationConfiguration } from '../models/application-configuration';
import { Config } from '../models/config';
import { ConfigStateService } from '../services/config-state.service';
import { ConfigState } from '../states';

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
    email: null,
    roles: [],
  } as ApplicationConfiguration.CurrentUser,
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
    store = spectator.inject(Store);
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

  test('should have a dispatch method for every ConfigState action', () => {
    const reg = /(?<=dispatch)(\w+)(?=\()/gm;
    ConfigStateService.toString()
      .match(reg)
      .forEach(fnName => {
        expect(ConfigActions[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'dispatch');
        spy.mockClear();

        const params = Array.from(new Array(ConfigActions[fnName].length));

        service[`dispatch${fnName}`](...params);
        expect(spy).toHaveBeenCalledWith(new ConfigActions[fnName](...params));
      });
  });
});
