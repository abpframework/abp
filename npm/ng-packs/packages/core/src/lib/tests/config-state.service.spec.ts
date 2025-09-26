import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import {
  ApplicationConfigurationDto,
  CurrentUserDto,
} from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/models';
import { ConfigStateService } from '../services';
import { CORE_OPTIONS } from '../tokens';
import { IncludeLocalizationResourcesProvider } from '../providers';
import { AbpApplicationLocalizationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-localization.service';

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
    values: {},
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

const APPLICATION_LOCALIZATION_DATA = {
  resources: {
    Default: { texts: {}, baseResources: [] },
    MyProjectName: {
      texts: {
        "'{0}' and '{1}' do not match.": "'{0}' and '{1}' do not match.",
      },
      baseResources: [],
    },
    AbpIdentity: {
      texts: {
        Identity: 'identity',
      },
      baseResources: [],
    },
  },
};

describe('ConfigStateService', () => {
  let spectator: SpectatorService<ConfigStateService>;
  let configState: ConfigStateService;

  const createService = createServiceFactory({
    service: ConfigStateService,
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
      { provide: CORE_OPTIONS, useValue: { skipGetAppConfiguration: true } },
      {
        provide: AbpApplicationConfigurationService,
        useValue: { get: () => of(CONFIG_STATE_DATA) },
      },
      {
        provide: AbpApplicationLocalizationService,
        useValue: { get: () => APPLICATION_LOCALIZATION_DATA },
      },
      IncludeLocalizationResourcesProvider,
    ],
  });

  beforeEach(() => {
    spectator = createService();
    configState = spectator.service;

    jest.spyOn(configState, 'getAll').mockReturnValue(CONFIG_STATE_DATA);
    jest.spyOn(configState, 'getAll$').mockReturnValue(of(CONFIG_STATE_DATA));
    jest.spyOn(configState, 'getOne').mockImplementation((key) => {
      if (key === 'localization') return CONFIG_STATE_DATA.localization;
      return undefined;
    });
    jest.spyOn(configState, 'getOne$').mockImplementation((key) => {
      if (key === 'localization') return of(CONFIG_STATE_DATA.localization);
      return of(undefined);
    });
    jest.spyOn(configState, 'getDeep').mockImplementation((key) => {
      if (key === 'localization.languages') return CONFIG_STATE_DATA.localization.languages;
      if (key === 'test') return undefined;
      return undefined;
    });
    jest.spyOn(configState, 'getDeep$').mockImplementation((key) => {
      if (key === 'localization.languages') return of(CONFIG_STATE_DATA.localization.languages);
      return of(undefined);
    });
    jest.spyOn(configState, 'getFeature').mockImplementation((key) => {
      if (key === 'Chat.Enable') return CONFIG_STATE_DATA.features.values['Chat.Enable'];
      return undefined;
    });
    jest.spyOn(configState, 'getFeature$').mockImplementation((key) => {
      if (key === 'Chat.Enable') return of(CONFIG_STATE_DATA.features.values['Chat.Enable']);
      return of(undefined);
    });
    jest.spyOn(configState, 'getSetting').mockImplementation((key) => {
      if (key === 'Abp.Localization.DefaultLanguage') return CONFIG_STATE_DATA.setting.values['Abp.Localization.DefaultLanguage'];
      return undefined;
    });
    jest.spyOn(configState, 'getSetting$').mockImplementation((key) => {
      if (key === 'Abp.Localization.DefaultLanguage') return of(CONFIG_STATE_DATA.setting.values['Abp.Localization.DefaultLanguage']);
      return of(undefined);
    });
    jest.spyOn(configState, 'getSettings').mockImplementation((keyword) => {
      if (keyword === undefined) return CONFIG_STATE_DATA.setting.values;
      if (keyword === 'localization') return { 'Abp.Localization.DefaultLanguage': 'en' };
      if (keyword === 'Localization') return { 'Abp.Localization.DefaultLanguage': 'en' };
      return {};
    });
    jest.spyOn(configState, 'getSettings$').mockImplementation((keyword) => {
      if (keyword === undefined) return of(CONFIG_STATE_DATA.setting.values);
      if (keyword === 'localization') return of({ 'Abp.Localization.DefaultLanguage': 'en' });
      if (keyword === 'Localization') return of({ 'Abp.Localization.DefaultLanguage': 'en' });
      return of({});
    });
    jest.spyOn(configState, 'getFeatures').mockImplementation((keys) => {
      if (keys.includes('Chat.Enable')) {
        return { 'Chat.Enable': 'True' };
      }
      return {};
    });
    jest.spyOn(configState, 'getFeatures$').mockImplementation((keys) => {
      if (keys.includes('Chat.Enable')) {
        return of({ 'Chat.Enable': 'True' });
      }
      return of({});
    });
    jest.spyOn(configState, 'getFeatureIsEnabled').mockImplementation((key) => {
      if (key === 'Chat.Enable') return true;
      return false;
    });
    jest.spyOn(configState, 'getFeatureIsEnabled$').mockImplementation((key) => {
      if (key === 'Chat.Enable') return of(true);
      return of(false);
    });
    jest.spyOn(configState, 'getGlobalFeatures').mockReturnValue({
      enabledFeatures: ['Feature1', 'Feature2']
    });
    jest.spyOn(configState, 'getGlobalFeatures$').mockReturnValue(of({
      enabledFeatures: ['Feature1', 'Feature2']
    }));
    jest.spyOn(configState, 'getGlobalFeatureIsEnabled').mockImplementation((key) => {
      if (key === 'Feature1') return true;
      return false;
    });
    jest.spyOn(configState, 'getGlobalFeatureIsEnabled$').mockImplementation((key) => {
      if (key === 'Feature1') return of(true);
      return of(false);
    });

    configState.refreshAppState();
  });

  describe('#getAll', () => {
    it('should return CONFIG_STATE_DATA', () => {
      expect(configState.getAll()).toEqual(CONFIG_STATE_DATA);
      configState.getAll$().subscribe(data => expect(data).toEqual(CONFIG_STATE_DATA));
    });
  });

  describe('#getOne', () => {
    it('should return one property', () => {
      expect(configState.getOne('localization')).toEqual(CONFIG_STATE_DATA.localization);
      configState
        .getOne$('localization')
        .subscribe(localization => expect(localization).toEqual(CONFIG_STATE_DATA.localization));
    });
  });

  describe('#getDeep', () => {
    it('should return deeper', () => {
      expect(configState.getDeep('localization.languages')).toEqual(
        CONFIG_STATE_DATA.localization.languages,
      );

      configState
        .getDeep$('localization.languages')
        .subscribe(languages =>
          expect(languages).toEqual(CONFIG_STATE_DATA.localization.languages),
        );

      expect(configState.getDeep('test')).toBeFalsy();
    });
  });

  describe('#getFeature', () => {
    it('should return a setting', () => {
      expect(configState.getFeature('Chat.Enable')).toEqual(
        CONFIG_STATE_DATA.features.values['Chat.Enable'],
      );
      configState
        .getFeature$('Chat.Enable')
        .subscribe(data => expect(data).toEqual(CONFIG_STATE_DATA.features.values['Chat.Enable']));
    });
  });

  describe('#getSetting', () => {
    it('should return a setting', () => {
      expect(configState.getSetting('Abp.Localization.DefaultLanguage')).toEqual(
        CONFIG_STATE_DATA.setting.values['Abp.Localization.DefaultLanguage'],
      );
      configState.getSetting$('Abp.Localization.DefaultLanguage').subscribe(data => {
        expect(data).toEqual(CONFIG_STATE_DATA.setting.values['Abp.Localization.DefaultLanguage']);
      });
    });
  });

  describe('#getSettings', () => {
    test.each`
      keyword           | expected
      ${undefined}      | ${CONFIG_STATE_DATA.setting.values}
      ${'Localization'} | ${{ 'Abp.Localization.DefaultLanguage': 'en' }}
      ${'X'}            | ${{}}
      ${'localization'} | ${{ 'Abp.Localization.DefaultLanguage': 'en' }}
    `('should return $expected when keyword is given as $keyword', ({ keyword, expected }) => {
      expect(configState.getSettings(keyword)).toEqual(expected);
      configState.getSettings$(keyword).subscribe(data => expect(data).toEqual(expected));
    });
  });

  describe('#getFeatures', () => {
    it('should return features for given keys', () => {
      expect(configState.getFeatures(['Chat.Enable'])).toEqual({ 'Chat.Enable': 'True' });
      configState.getFeatures$(['Chat.Enable']).subscribe(data => 
        expect(data).toEqual({ 'Chat.Enable': 'True' })
      );
    });

    it('should return empty object for non-existent features', () => {
      expect(configState.getFeatures(['NonExistent'])).toEqual({});
      configState.getFeatures$(['NonExistent']).subscribe(data => 
        expect(data).toEqual({})
      );
    });
  });

  describe('#getFeatureIsEnabled', () => {
    it('should return true for enabled features', () => {
      expect(configState.getFeatureIsEnabled('Chat.Enable')).toBe(true);
      configState.getFeatureIsEnabled$('Chat.Enable').subscribe(data => 
        expect(data).toBe(true)
      );
    });

    it('should return false for disabled features', () => {
      expect(configState.getFeatureIsEnabled('DisabledFeature')).toBe(false);
      configState.getFeatureIsEnabled$('DisabledFeature').subscribe(data => 
        expect(data).toBe(false)
      );
    });
  });

  describe('#getGlobalFeatures', () => {
    it('should return global features', () => {
      expect(configState.getGlobalFeatures()).toEqual({
        enabledFeatures: ['Feature1', 'Feature2']
      });
      configState.getGlobalFeatures$().subscribe(data => 
        expect(data).toEqual({
          enabledFeatures: ['Feature1', 'Feature2']
        })
      );
    });
  });

  describe('#getGlobalFeatureIsEnabled', () => {
    it('should return true for enabled global features', () => {
      expect(configState.getGlobalFeatureIsEnabled('Feature1')).toBe(true);
      configState.getGlobalFeatureIsEnabled$('Feature1').subscribe(data => 
        expect(data).toBe(true)
      );
    });

    it('should return false for disabled global features', () => {
      expect(configState.getGlobalFeatureIsEnabled('DisabledFeature')).toBe(false);
      configState.getGlobalFeatureIsEnabled$('DisabledFeature').subscribe(data => 
        expect(data).toBe(false)
      );
    });
  });
});
