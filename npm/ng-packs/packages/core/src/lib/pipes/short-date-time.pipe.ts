import { DatePipe, DATE_PIPE_DEFAULT_TIMEZONE } from '@angular/common';
import { LOCALE_ID, Pipe, PipeTransform, inject } from '@angular/core';
import { ConfigStateService } from '../services';
import { getShortDateShortTimeFormat } from '../utils/date-utils';

@Pipe({
  name: 'shortDateTime',
  pure: true,
})
export class ShortDateTimePipe extends DatePipe implements PipeTransform {
  private configStateService = inject(ConfigStateService);

  constructor() {
    const locale = inject(LOCALE_ID);
    const defaultTimezone = inject(DATE_PIPE_DEFAULT_TIMEZONE, { optional: true });

    super(locale, defaultTimezone);
  }

  transform(
    value: Date | string | number,
    format?: string,
    timezone?: string,
    locale?: string,
  ): string | null;
  transform(value: null | undefined, format?: string, timezone?: string, locale?: string): null;
  transform(
    value: string | number | Date | null | undefined,
    timezone?: string,
    locale?: string,
  ): string | null {
    const format = getShortDateShortTimeFormat(this.configStateService);
    return super.transform(value, format, timezone, locale);
  }
}
