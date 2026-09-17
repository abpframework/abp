import { inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { ConfigStateService } from './config-state.service';

@Injectable({
  providedIn: 'root',
})
export class TimezoneService {
  protected readonly configState = inject(ConfigStateService);
  protected readonly document = inject(DOCUMENT);
  private readonly cookieKey = '__timezone';
  private timeZoneNameFromSettings: string | null | undefined;
  public isUtcClockEnabled: boolean | undefined;

  constructor() {
    this.configState.getOne$('setting').subscribe(settings => {
      this.timeZoneNameFromSettings = settings?.values?.['Abp.Timing.TimeZone'];
    });
    this.configState.getOne$('clock').subscribe(clock => {
      this.isUtcClockEnabled = clock?.kind === 'Utc';
    });
  }

  /**
   * Returns the effective timezone to be used across the application.
   *
   * This value is determined based on the clock kind setting in the configuration:
   * - If clock kind is not equal to Utc, the browser's local timezone is returned.
   * - If clock kind is equal to Utc, the configured timezone (`timeZoneNameFromSettings`) is returned if available;
   *   otherwise, the browser's timezone is used as a fallback.
   *
   * @returns The IANA timezone name (e.g., 'Europe/Istanbul', 'America/New_York').
   */
  get timezone(): string {
    if (!this.isUtcClockEnabled) {
      return this.getBrowserTimezone();
    }
    return this.timeZoneNameFromSettings || this.getBrowserTimezone();
  }

  /**
   * Retrieves the browser's local timezone based on the user's system settings.
   *
   * @returns The IANA timezone name (e.g., 'Europe/Istanbul', 'America/New_York').
   */
  getBrowserTimezone(): string {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
  }

  /**
   * Sets the application's timezone in a cookie to persist the user's selected timezone.
   *
   * This method sets the cookie only if the clock kind setting is set to UTC.
   * The cookie is stored using the key defined by `this.cookieKey` and applied to the root path (`/`).
   *
   * @param timezone - The IANA timezone name to be stored (e.g., 'Europe/Istanbul').
   */
  setTimezone(timezone: string): void {
    if (this.isUtcClockEnabled) {
      this.document.cookie = `${this.cookieKey}=${timezone}; path=/`;
    }
  }
}
