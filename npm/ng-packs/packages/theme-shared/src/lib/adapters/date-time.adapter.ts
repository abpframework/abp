import { Injectable } from '@angular/core';
import { NgbDateStruct, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class DateTimeAdapter {
  value!: Partial<NgbDateTimeStruct>;

  fromModel(value: string | Date): Partial<NgbDateTimeStruct> | null {
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

  toModel(value: Partial<NgbDateTimeStruct> | null): string {
    if (!value) {
      return '';
    }

    const date = new Date(
      Date.UTC(
        value.year,
        value.month - 1,
        value.day,
        value?.hour || 0,
        value?.minute || 0,
        value?.second || 0,
      ),
    );

    return date.toISOString();
  }
}

type NgbDateTimeStruct = NgbDateStruct & NgbTimeStruct;
