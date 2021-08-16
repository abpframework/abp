import { CoreTestingModule } from '@abp/ng.core/testing';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import {
  ApplicationConfigurationDto,
  CurrentUserDto,
} from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import { ConfigStateService } from '../services';
import { CORE_OPTIONS } from '../tokens';

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
    roles: [],
  } as CurrentUserDto,
  features: {
    values: {
      'Chat.Enable': 'True',
    },
  },
  registerLocaleFn: () => Promise.resolve(),
} as any as ApplicationConfigurationDto;

describe('ConfigState', () => {
  let spectator: SpectatorService<ConfigStateService>;
  let configState: ConfigStateService;

  const createService = createServiceFactory({
    service: ConfigStateService,
    imports: [CoreTestingModule.withConfig()],
    providers: [
      { provide: CORE_OPTIONS, useValue: { skipGetAppConfiguration: true } },
      { provide: Store, useValue: {} },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    configState = spectator.service;

    configState.setState(CONFIG_STATE_DATA);
  });

  describe('#getAll', () => {
    it('should return CONFIG_STATE_DATA', () => {
      expect(configState.getAll()).toEqual(CONFIG_STATE_DATA);
      configState
        .getAll$()
        .subscribe((data) => expect(data).toEqual(CONFIG_STATE_DATA));
    });
  });

  describe('#getOne', () => {
    it('should return one property', () => {
      expect(configState.getOne('localization')).toEqual(
        CONFIG_STATE_DATA.localization
      );
      configState
        .getOne$('localization')
        .subscribe((localization) =>
          expect(localization).toEqual(CONFIG_STATE_DATA.localization)
        );
    });
  });

  describe('#getDeep', () => {
    it('should return deeper', () => {
      expect(configState.getDeep('localization.languages')).toEqual(
        CONFIG_STATE_DATA.localization.languages
      );

      configState
        .getDeep$('localization.languages')
        .subscribe((languages) =>
          expect(languages).toEqual(CONFIG_STATE_DATA.localization.languages)
        );

      expect(configState.getDeep('test')).toBeFalsy();
    });
  });

  describe('#getFeature', () => {
    it('should return a setting', () => {
      expect(configState.getFeature('Chat.Enable')).toEqual(
        CONFIG_STATE_DATA.features.values['Chat.Enable']
      );
      configState
        .getFeature$('Chat.Enable')
        .subscribe((data) =>
          expect(data).toEqual(CONFIG_STATE_DATA.features.values['Chat.Enable'])
        );
    });
  });

  describe('#getSetting', () => {
    it('should return a setting', () => {
      expect(
        configState.getSetting('Abp.Localization.DefaultLanguage')
      ).toEqual(
        CONFIG_STATE_DATA.setting.values['Abp.Localization.DefaultLanguage']
      );
      configState
        .getSetting$('Abp.Localization.DefaultLanguage')
        .subscribe((data) => {
          expect(data).toEqual(
            CONFIG_STATE_DATA.setting.values['Abp.Localization.DefaultLanguage']
          );
        });
    });
  });

  describe('#getSettings', () => {
    test.each`
      keyword           | expected
      ${undefined}      | ${CONFIG_STATE_DATA.setting.values}
      ${'Localization'} | ${{ 'Abp.Localization.DefaultLanguage': 'en' }}
      ${'X'}            | ${{}}
      ${'localization'} | ${{}}
    `(
      'should return $expected when keyword is given as $keyword',
      ({ keyword, expected }) => {
        expect(configState.getSettings(keyword)).toEqual(expected);
        configState
          .getSettings$(keyword)
          .subscribe((data) => expect(data).toEqual(expected));
      }
    );
  });
});
