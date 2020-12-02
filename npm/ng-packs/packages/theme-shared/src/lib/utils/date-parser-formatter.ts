import { ApplicationLocalizationConfigurationDto, ConfigStateService } from '@abp/ng.core';
import { DatePipe } from '@angular/common';
import { Injectable, Optional } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

function padNumber(value: number) {
  if (isNumber(value)) {
    return `0${value}`.slice(-2);
  } else {
    return '';
  }
}

function isNumber(value: any): boolean {
  return !isNaN(toInteger(value));
}

function toInteger(value: any): number {
  return parseInt(`${value}`, 10);
}

@Injectable()
export class DateParserFormatter extends NgbDateParserFormatter {
  constructor(@Optional() private datePipe: DatePipe, private configState: ConfigStateService) {
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
    const { shortDatePattern } = (this.configState.getOne(
      'localization',
    ) as ApplicationLocalizationConfigurationDto).currentCulture.dateTimeFormat;

    if (date && this.datePipe) {
      return this.datePipe.transform(
        new Date(date.year, date.month - 1, date.day),
        shortDatePattern,
      );
    } else {
      return date
        ? `${date.year}-${isNumber(date.month) ? padNumber(date.month) : ''}-${
            isNumber(date.day) ? padNumber(date.day) : ''
          }`
        : '';
    }
  }
}
