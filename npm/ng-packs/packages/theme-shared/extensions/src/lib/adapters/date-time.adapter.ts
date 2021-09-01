import { Injectable } from '@angular/core';
import { NgbDateStruct, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class DateTimeAdapter {
  value: NgbDateTimeStruct;

  fromModel(value: string | Date): NgbDateTimeStruct | null {
    if (!value) return null;

    const date = new Date(value);

    if (isNaN(date as unknown as number)) return null;

    this.value = {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate(),
      hour: date.getHours(),
      minute: date.getMinutes(),
      second: date.getSeconds(),
    };

    return this.value;
  }

  toModel(value: NgbDateTimeStruct | null): string {
    if (!value) return '';

    const now = new Date();

    value = {
      year: now.getUTCFullYear(),
      month: now.getMonth() + 1,
      day: now.getDate(),
      hour: 0,
      minute: 0,
      second: 0,
      ...this.value,
      ...value,
    };

    const date = new Date(
      value.year,
      value.month - 1,
      value.day,
      value.hour,
      value.minute,
      value.second,
    );

    return new Date(date).toISOString();
  }
}

type NgbDateTimeStruct = NgbDateStruct & NgbTimeStruct;
