import { DatePipe, DATE_PIPE_DEFAULT_TIMEZONE } from '@angular/common';
import { Inject, LOCALE_ID, Optional, Pipe, PipeTransform } from '@angular/core';
import { ConfigStateService } from '../services';
import { getShortTimeFormat } from '../utils/date-utils';

@Pipe({
  name: 'shortTime',
  pure: true,
})
export class ShortTimePipe extends DatePipe implements PipeTransform {

  constructor(private configStateService: ConfigStateService,
    @Inject(LOCALE_ID) locale: string,
    @Inject(DATE_PIPE_DEFAULT_TIMEZONE) @Optional()  defaultTimezone?: string|null
    ) {
    super(locale, defaultTimezone)
  }

  transform(value: Date | string | number, format?: string, timezone?: string, locale?: string): string | null;
  transform(value: null | undefined, format?: string, timezone?: string, locale?: string): null;
  transform(
    value: string|number|Date|null|undefined, timezone?: string,
    locale?: string): string|null {

  const format = getShortTimeFormat(this.configStateService);
  return super.transform(value,format,timezone,locale)
  }


}


