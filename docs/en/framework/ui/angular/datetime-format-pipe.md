```json
//[doc-seo]
{
    "Description": "Format dates and handle clock-aware timezone conversion in ABP Angular applications with pipes, TimeService, and TimezoneService."
}
```

{%{
# Date and Time

ABP Angular provides culture-aware format pipes, clock-aware UTC conversion, timezone selection, and date-time services. These APIs use the culture, clock, and timezone values from the application configuration.

## Culture-Aware Format Pipes

Angular's built-in `DatePipe` can format a date directly:


```html
<span>{{ today | date:'dd/MM/yy' }}</span>
```

The ABP pipes below use the short date and time patterns returned in the application localization configuration.

### `shortDate`

```html
<span>{{ today | shortDate }}</span>
```

### `shortTime`

```html
<span>{{ today | shortTime }}</span>
```

### `shortDateTime`

```html
<span>{{ today | shortDateTime }}</span>
```

These pipes extend Angular's `DatePipe`. They select the format pattern from `ConfigStateService`; they do not apply ABP's clock-aware timezone selection. Use `abpUtcToLocal` when the value also needs to follow the application's clock and timezone.

## Clock-Aware UTC Conversion

The `abpUtcToLocal` pipe accepts `date`, `time`, or `datetime` as its format type:

```html
<span>{{ order.creationTime | abpUtcToLocal:'datetime' }}</span>
```

Its behavior depends on the clock configuration returned by the backend:

- With the UTC clock enabled, it converts the input to `TimezoneService.timezone`, including the daylight-saving-time offset for that date.
- With a non-UTC clock, it formats the value without applying the configured timezone conversion.
- Empty or invalid input produces an empty string.

The output pattern is still taken from the current application's short date and time formats.

## `TimezoneService`

Inject `TimezoneService` to read or persist the timezone used by the Angular application:

```ts
import { TimezoneService } from '@abp/ng.core';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-timezone-selector',
  template: `<button type="button" (click)="selectTimezone()">Use Istanbul time</button>`,
})
export class TimezoneSelectorComponent {
  private readonly timezoneService = inject(TimezoneService);

  selectTimezone(): void {
    this.timezoneService.setTimezone('Europe/Istanbul');
  }
}
```

`timezone` returns:

- the browser timezone when the backend clock is not UTC;
- the `Abp.Timing.TimeZone` setting when the clock is UTC and the setting has a value;
- the browser timezone as a fallback when the UTC setting is empty.

`setTimezone` writes the selected IANA timezone to the `__timezone` cookie only when the UTC clock is enabled.

When you configure the application with `provideAbpCore`, its built-in `timezoneInterceptor` adds the effective timezone to outgoing `HttpClient` requests as the `__timezone` header. It does not add the header when the UTC clock is disabled.

## `TimeService`

`TimeService` returns [Luxon](https://moment.github.io/luxon/#/) `DateTime` values and formats dates with the current Angular locale:

```ts
import { TimeService } from '@abp/ng.core';
import { inject, Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ScheduleFormatter {
  private readonly timeService = inject(TimeService);

  formatForIstanbul(value: string): string {
    return this.timeService.format(value, 'ff', 'Europe/Istanbul');
  }
}
```

| Method | Behavior |
| --- | --- |
| `now(zone = 'local')` | Returns the current time in the requested IANA timezone. |
| `toZone(value, zone)` | Parses an ISO string or `Date` and returns a Luxon value in the requested timezone. |
| `format(value, format = 'ff', zone = 'local')` | Converts to the timezone, applies its DST rules, and formats with the current locale. |
| `formatDateWithStandardOffset(value, format = 'ff', zone?)` | Applies the zone's January 1 offset and formats without any further timezone or DST conversion. |
| `formatWithoutTimeZone(value, format = 'ff')` | Formats the parsed ISO clock fields without shifting them to another timezone. |

}%}
