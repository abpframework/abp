import { formatDate } from '@angular/common';
import { inject, Injectable, LOCALE_ID } from '@angular/core';
import { NgbDatepickerI18n, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ConfigStateService } from '@abp/ng.core';

@Injectable()
export class DatepickerI18nAdapter extends NgbDatepickerI18n {
  private configState = inject(ConfigStateService, { optional: true });
  private defaultLocale = inject(LOCALE_ID);

  private get locale(): string {
    return this.configState?.getDeep('localization.currentCulture.cultureName') || this.defaultLocale;
  }

  getWeekdayLabel(weekday: number): string {
    const date = new Date(2017, 0, weekday + 1); // Monday = 1
    return formatDate(date, 'EEEEE', this.locale);
  }

  getWeekLabel(): string {
    return '';
  }

  getMonthShortName(month: number): string {
    const date = new Date(2017, month - 1, 1);
    return formatDate(date, 'MMM', this.locale);
  }

  getMonthFullName(month: number): string {
    const date = new Date(2017, month - 1, 1);
    return formatDate(date, 'MMMM', this.locale);
  }

  getDayAriaLabel(date: NgbDateStruct): string {
    const d = new Date(date.year, date.month - 1, date.day);
    return formatDate(d, 'fullDate', this.locale);
  }
}
