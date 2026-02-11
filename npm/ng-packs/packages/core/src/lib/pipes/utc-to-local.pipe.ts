import { Pipe, PipeTransform, Injectable, inject, LOCALE_ID } from '@angular/core';
import { ConfigStateService, LocalizationService, TimeService, TimezoneService } from '../services';
import { getShortDateFormat, getShortDateShortTimeFormat, getShortTimeFormat } from '../utils';

@Injectable()
@Pipe({
  name: 'abpUtcToLocal',
})
export class UtcToLocalPipe implements PipeTransform {
  protected readonly timezoneService = inject(TimezoneService);
  protected readonly timeService = inject(TimeService);
  protected readonly configState = inject(ConfigStateService);
  protected readonly localizationService = inject(LocalizationService);
  protected readonly locale = inject(LOCALE_ID);

  transform(
    value: string | Date | null | undefined,
    type: 'date' | 'datetime' | 'time',
  ): string | Date {
    if (!value) return '';

    const date = new Date(value);
    if (isNaN(date.getTime())) return '';

    const format = this.getFormat(type);

    try {
      if (this.timezoneService.isUtcClockEnabled) {
        const timeZone = this.timezoneService.timezone;
        return this.timeService.formatDateWithStandardOffset(date, format, timeZone);
      } else {
        return this.timeService.formatWithoutTimeZone(date, format);
      }
    } catch (err) {
      return value;
    }
  }

  private getFormat(propType: 'date' | 'datetime' | 'time'): string {
    switch (propType) {
      case 'date':
        return getShortDateFormat(this.configState);
      case 'time':
        return getShortTimeFormat(this.configState);
      case 'datetime':
      default:
        return getShortDateShortTimeFormat(this.configState);
    }
  }
}
