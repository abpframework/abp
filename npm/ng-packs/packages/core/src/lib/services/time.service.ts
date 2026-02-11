import { inject, Injectable, LOCALE_ID } from '@angular/core';
import { DateTime } from 'luxon';

@Injectable({
  providedIn: 'root',
})
export class TimeService {
  private locale = inject(LOCALE_ID);

  /**
   * Returns the current date and time in the specified timezone.
   *
   * @param zone - An IANA timezone name (e.g., 'Europe/Istanbul', 'UTC'); defaults to the system's local timezone.
   * @returns A Luxon DateTime instance representing the current time in the given timezone.
   */
  now(zone = 'local'): DateTime {
    return DateTime.now().setZone(zone);
  }

  /**
   * Converts the input date to the specified timezone, applying any timezone and daylight saving time (DST) adjustments.
   *
   * This method:
   * 1. Parses the input value into a Luxon DateTime object.
   * 2. Applies the specified IANA timezone, including any DST shifts based on the given date.
   *
   * @param value - The ISO string or Date object to convert.
   * @param zone - An IANA timezone name (e.g., 'America/New_York').
   * @returns A Luxon DateTime instance adjusted to the specified timezone and DST rules.
   */
  toZone(value: string | Date, zone: string): DateTime {
    return DateTime.fromISO(value instanceof Date ? value.toISOString() : value, {
      zone,
    });
  }

  /**
   * Formats the input date by applying timezone and daylight saving time (DST) adjustments.
   *
   * This method:
   * 1. Converts the input date to the specified timezone.
   * 2. Formats the result using the given format and locale, reflecting any timezone or DST shifts.
   *
   * @param value - The ISO string or Date object to format.
   * @param format - The format string (default: 'ff').
   * @param zone - Optional IANA timezone name (e.g., 'America/New_York'); defaults to the system's local timezone.
   * @returns A formatted date string adjusted for the given timezone and DST rules.
   */
  format(value: string | Date, format = 'ff', zone = 'local'): string {
    return this.toZone(value, zone).setLocale(this.locale).toFormat(format);
  }

  /**
   * Formats a date using the standard time offset (ignoring daylight saving time) for the specified timezone.
   *
   * This method:
   * 1. Converts the input date to UTC.
   * 2. Calculates the standard UTC offset for the given timezone (based on January 1st to avoid DST).
   * 3. Applies the standard offset manually to the UTC time.
   * 4. Formats the result using the specified format and locale, without applying additional timezone shifts.
   *
   * @param value - The ISO string or Date object to format.
   * @param format - The Luxon format string (default: 'ff').
   * @param zone - Optional IANA timezone name (e.g., 'America/New_York'); if omitted, system local timezone is used.
   * @returns A formatted date string adjusted by standard time (non-DST).
   */
  formatDateWithStandardOffset(value: string | Date, format = 'ff', zone?: string): string {
    const utcDate =
      typeof value === 'string'
        ? DateTime.fromISO(value, { zone: 'UTC' })
        : DateTime.fromJSDate(value, { zone: 'UTC' });

    if (!utcDate.isValid) return '';

    const targetZone = zone ?? DateTime.local().zoneName;

    const januaryDate = DateTime.fromObject(
      { year: utcDate.year, month: 1, day: 1 },
      { zone: targetZone },
    );
    const standardOffset = januaryDate.offset;
    const dateWithStandardOffset = utcDate.plus({ minutes: standardOffset });

    return dateWithStandardOffset.setZone('UTC').setLocale(this.locale).toFormat(format);
  }

  /**
   * Formats the input date using its original clock time, without converting based on timezone or DST
   *
   * This method:
   * 1. Converts the input date to ISO string.
   * 2. Calculates the date time in UTC, keeping the local time.
   * 3. Formats the result using the specified format and locale, without shifting timezones.
   *
   * @param value - The ISO string or Date object to format.
   * @param format - The format string (default: 'ff').
   * @returns A formatted date string without applying timezone.
   */
  formatWithoutTimeZone(value: string | Date, format = 'ff'): string {
    const isoString = value instanceof Date ? value.toISOString() : value;

    const dateTime = DateTime.fromISO(isoString)
      .setZone('utc', { keepLocalTime: true })
      .setLocale(this.locale);
    return dateTime.toFormat(format);
  }
}
