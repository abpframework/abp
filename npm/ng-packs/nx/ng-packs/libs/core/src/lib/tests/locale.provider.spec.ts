import { Component, LOCALE_ID } from '@angular/core';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { differentLocales } from '../constants/different-locales';
import { LocaleId } from '../providers';
import { LocalizationService } from '../services';

@Component({ selector: 'abp-dummy', template: '' })
export class DummyComponent {}

describe('LocaleProvider', () => {
  let spectator: SpectatorRouting<DummyComponent>;

  const createComponent = createRoutingFactory({
    component: DummyComponent,
    stubsEnabled: false,
    providers: [
      { provide: LocalizationService, useValue: { currentLang: 'en-US' } },
      {
        provide: LOCALE_ID,
        useClass: LocaleId,
        deps: [LocalizationService],
      },
    ],
  });

  describe('#LOCALE_ID', () => {
    test('should equal to currentLang', async () => {
      spectator = createComponent();
      const localizationService = spectator.inject(LocalizationService);

      expect(spectator.inject(LOCALE_ID).valueOf()).toBe(differentLocales['en-US'] || 'en-US');

      (localizationService as any).currentLang = 'tr';
      expect(spectator.inject(LOCALE_ID).valueOf()).toBe(differentLocales['tr'] || 'tr');
    });
  });
});
