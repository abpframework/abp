import { Component, LOCALE_ID } from '@angular/core';
import { createRoutingFactory, SpectatorHost, SpectatorRouting } from '@ngneat/spectator/jest';
import { LocalizationService } from '../services';
import { LocaleProvider, LocaleId } from '../providers';
import localesMapping from '../constants/different-locales';

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
      const localizationService = spectator.get(LocalizationService);

      expect(spectator.get(LOCALE_ID).valueOf()).toBe(localesMapping['en-US'] || 'en-US');

      (localizationService as any).currentLang = 'tr';
      expect(spectator.get(LOCALE_ID).valueOf()).toBe(localesMapping['tr'] || 'tr');
    });
  });
});
