import { Injectable, Optional } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';

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
  constructor(@Optional() private datePipe: DatePipe) {
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
    if (date && this.datePipe) {
      return this.datePipe.transform(new Date(date.year, date.month - 1, date.day), 'shortDate');
    } else {
      return date
        ? `${date.year}-${isNumber(date.month) ? padNumber(date.month) : ''}-${
            isNumber(date.day) ? padNumber(date.day) : ''
          }`
        : '';
    }
  }
}
