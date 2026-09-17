import { LOCALE_ID, Provider, inject } from '@angular/core';
import { differentLocales } from '../constants/different-locales';
import { LocalizationService } from '../services/localization.service';
import { checkHasProp } from '../utils/common-utils';

export class LocaleId extends String {
  private localizationService = inject(LocalizationService);

  constructor() {
    super();
  }

  toString(): string {
    const { currentLang } = this.localizationService;
    if (checkHasProp(differentLocales, currentLang)) {
      return differentLocales[currentLang];
    }
    return currentLang;
  }

  valueOf(): string {
    return this.toString();
  }
}

export const LocaleProvider: Provider = {
  provide: LOCALE_ID,
  useClass: LocaleId,
};