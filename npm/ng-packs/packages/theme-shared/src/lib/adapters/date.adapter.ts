import { formatDate } from '@angular/common';
import { Injectable } from '@angular/core';
import { NgbDateAdapter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class DateAdapter extends NgbDateAdapter<string | Date> {
  fromModel(value: string | Date): NgbDateStruct | null {
    if (!value) return null;

    let date: Date;

    if (typeof value === 'string') {
      date = this.dateOf(value);
    } else {
      date = new Date(value);
    }

    if (isNaN(date as unknown as number)) return null;

    return {
      day: date.getDate(),
      month: date.getMonth() + 1,
      year: date.getFullYear(),
    };
  }

  toModel(value: NgbDateStruct | null): string {
    if (!value) return '';

    const date = new Date(value.year, value.month - 1, value.day);
    const formattedDate = formatDate(date, 'yyyy-MM-dd', 'en');

    return formattedDate;
  }

  protected dateOf(value: string): Date {
    const dateUtc = new Date(Date.parse(value));
    return new Date(dateUtc.getTime() + Math.abs(dateUtc.getTimezoneOffset() * 60000));
  }
}
