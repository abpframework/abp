import { ApplicationLocalizationConfigurationDto, ConfigStateService } from '@abp/ng.core';
import { formatDate } from '@angular/common';
import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import snq from 'snq';

function isNumber(value: any): boolean {
  return !isNaN(toInteger(value));
}

function toInteger(value: any): number {
  return parseInt(`${value}`, 10);
}

@Injectable()
export class DateParserFormatter extends NgbDateParserFormatter {
  constructor(private configState: ConfigStateService, @Inject(LOCALE_ID) private locale: string) {
    super();
  }

  parse(value: string): NgbDateStruct {
    if (value) {
      const dateParts = value.trim().split('-');
      if (dateParts.length === 1 && isNumber(dateParts[0])) {
        return { year: toInteger(dateParts[0]), month: null, day: null };
      } else if (dateParts.length === 2 && isNumber(dateParts[0]) && isNumber(dateParts[1])) {
        return { year: toInteger(dateParts[0]), month: toInteger(dateParts[1]), day: null };
      } else if (
        dateParts.length === 3 &&
        isNumber(dateParts[0]) &&
        isNumber(dateParts[1]) &&
        isNumber(dateParts[2])
      ) {
        return {
          year: toInteger(dateParts[0]),
          month: toInteger(dateParts[1]),
          day: toInteger(dateParts[2]),
        };
      }
    }
    return null;
  }

  format(date: NgbDateStruct): string {
    if (!date) return '';

    const localization: ApplicationLocalizationConfigurationDto =
      this.configState.getOne('localization');

    const dateFormat =
      snq(() => localization.currentCulture.dateTimeFormat.shortDatePattern) || 'yyyy-MM-dd';

    return formatDate(new Date(date.year, date.month - 1, date.day), dateFormat, this.locale);
  }
}
