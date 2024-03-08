import { Injectable } from '@angular/core';
import { NgbDateStruct, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class DateTimeAdapter {
  value!: Partial<NgbDateTimeStruct>;

  fromModel(value: string | Date): Partial<NgbDateTimeStruct> | null {
    if (!value) {
      return null;
    }

    const date = new Date(value);

    if (isNaN(date as unknown as number)) {
      return null;
    }

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

  toModel(value: Partial<NgbDateTimeStruct> | null): string {
    if (!value) {
      return '';
    }

    const now = new Date();

    const newValue = {
      year: now.getUTCFullYear(),
      month: now.getMonth() + 1,
      day: now.getDate(),
      hour: 0,
      minute: 0,
      second: 0,
      ...this.value,
      ...value,
    } as NgbDateTimeStruct;

    const date = new Date(
      Date.UTC(
        newValue.year,
        newValue.month - 1,
        newValue.day,
        newValue.hour,
        newValue.minute,
        newValue.second,
      ),
    );

    return date.toISOString().replace('Z', '');
  }
}

type NgbDateTimeStruct = NgbDateStruct & NgbTimeStruct;
