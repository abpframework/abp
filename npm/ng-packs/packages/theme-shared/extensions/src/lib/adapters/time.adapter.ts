import { formatDate } from '@angular/common';
import { Injectable } from '@angular/core';
import { NgbTimeAdapter, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class TimeAdapter extends NgbTimeAdapter<string> {
  fromModel(value: string | Date): NgbTimeStruct | null {
    if (!value) return null;

    const date = isTimeStr(value)
      ? new Date(0, 0, 1, ...value.split(':').map(Number))
      : new Date(value);

    if (isNaN((date as unknown) as number)) return null;

    return {
      hour: date.getHours(),
      minute: date.getMinutes(),
      second: date.getSeconds(),
    };
  }

  toModel(value: NgbTimeStruct | null): string {
    if (!value) return '';

    const date = new Date(0, 0, 1, value.hour, value.minute, value.second);
    const formattedDate = formatDate(date, 'HH:mm', 'en');

    return formattedDate;
  }
}

function isTimeStr(value: string | Date): value is string {
  return /^((2[123])|[01][0-9])(\:[0-5][0-9]){1,2}$/.test(String(value));
}
