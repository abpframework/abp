import { formatDate } from '@angular/common';
import { inject, Injectable, LOCALE_ID } from '@angular/core';
import { NgbTimepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import { ConfigStateService } from '@abp/ng.core';

@Injectable()
export class TimepickerI18nAdapter extends NgbTimepickerI18n {
  private configState = inject(ConfigStateService, { optional: true });
  private defaultLocale = inject(LOCALE_ID);

  private get locale(): string {
    return this.configState?.getDeep('localization.currentCulture.cultureName') || this.defaultLocale;
  }

  getMorningPeriod(): string {
    const date = new Date(2000, 0, 1, 10, 0, 0);
    return formatDate(date, 'a', this.locale);
  }

  getAfternoonPeriod(): string {
    const date = new Date(2000, 0, 1, 22, 0, 0);
    return formatDate(date, 'a', this.locale);
  }
}
