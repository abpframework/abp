import { AbpApplicationConfigurationService, ConfigStateService } from '@abp/ng.core';
import { CoreTestingModule } from '@abp/ng.core/testing';
import { AbpApplicationLocalizationService } from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/vitest';
import { OAuthService } from 'angular-oauth2-oidc';
import { of } from 'rxjs';
import { getPasswordValidators, validatePassword } from '../utils';

@Component({ template: '', selector: 'abp-dummy' })
class DummyComponent {}

describe('ValidationUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    imports: [CoreTestingModule.withConfig()],
    mocks: [HttpClient, OAuthService],
    providers: [
      {
        provide: AbpApplicationConfigurationService,
        useValue: {
          get: () =>
            of({
              setting: {
                values: {
                  'Abp.Identity.Password.RequiredLength': '6',
                  'Abp.Identity.Password.RequiredUniqueChars': '1',
                  'Abp.Identity.Password.RequireNonAlphanumeric': 'True',
                  'Abp.Identity.Password.RequireLowercase': 'True',
                  'Abp.Identity.Password.RequireUppercase': 'True',
                  'Abp.Identity.Password.RequireDigit': 'True',
                },
              },
              localization: {
                values: {},
                languages: [],
                currentCulture: {
                  cultureName: 'en',
                  displayName: 'English',
                  englishName: 'English',
                  threeLetterIsoLanguageName: 'eng',
                  twoLetterIsoLanguageName: 'en',
                  isRightToLeft: false,
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
                resources: {},
                languagesMap: {},
                languageFilesMap: {},
              },
            }),
        },
      },
      {
        provide: AbpApplicationLocalizationService,
        useValue: {
          get: () =>
            of({
              resources: {
                Default: { texts: {}, baseResources: [] },
              },
            }),
        },
      },
    ],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getPasswordValidators', () => {
    it('should return password validators', () => {
      const configState = spectator.inject(ConfigStateService);
      configState.refreshAppState();

      const validators = getPasswordValidators(spectator.inject(Injector));
      
      expect(validators.length).toBeGreaterThan(0);
      
      const minLengthValidator = validators.find(v => v.toString().includes('minLength'));
      const maxLengthValidator = validators.find(v => v.toString().includes('maxLength'));
      
      expect(minLengthValidator).toBeDefined();
      expect(maxLengthValidator).toBeDefined();
    });
  });

  describe('#validatePassword', () => {
    it('should validate password rules correctly', () => {
      const numberValidator = validatePassword('number');
      const smallValidator = validatePassword('small');
      const capitalValidator = validatePassword('capital');
      const specialValidator = validatePassword('special');

        expect(numberValidator({ value: 'abc123' } as any)).toBeNull();
      expect(smallValidator({ value: 'abc123' } as any)).toBeNull();
      expect(capitalValidator({ value: 'ABC123' } as any)).toBeNull();
      expect(specialValidator({ value: 'abc@123' } as any)).toBeNull();

      expect(numberValidator({ value: 'abc' } as any)).toEqual({ passwordRequiresDigit: true });
      expect(smallValidator({ value: 'ABC123' } as any)).toEqual({ passwordRequiresLower: true });
      expect(capitalValidator({ value: 'abc123' } as any)).toEqual({ passwordRequiresUpper: true });
      expect(specialValidator({ value: 'abc123' } as any)).toEqual({ passwordRequiresNonAlphanumeric: true });
    });
  });
});
