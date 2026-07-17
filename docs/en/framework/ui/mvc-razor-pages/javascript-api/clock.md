```json
//[doc-seo]
{
    "Description": "Use ABP's MVC JavaScript Clock API for time-zone detection, date normalization, localized display, and the browser time-zone cookie."
}
```

# ASP.NET Core MVC / Razor Pages UI: JavaScript Clock API

The `abp.clock` namespace provides date, time-zone and browser-time-zone helpers for MVC / Razor Pages applications. The application configuration script sets `abp.clock.kind` from the server-side ABP clock configuration.

## Time-Zone Support

`abp.clock.supportsMultipleTimezone()` returns `true` when the configured clock kind is `Utc`. `abp.clock.timeZone()` returns the value of the `Abp.Timing.TimeZone` setting when it is available and otherwise returns the browser's IANA time-zone name.

````js
if (abp.clock.supportsMultipleTimezone()) {
    console.log(abp.clock.timeZone());
}
````

## Normalize Date Values

Use `normalizeToString` before sending a date value to the server and `normalizeToLocaleString` before displaying a server value:

````js
const requestValue = abp.clock.normalizeToString(new Date());
const displayValue = abp.clock.normalizeToLocaleString(requestValue);
````

`normalizeToString` returns an ISO-compatible date-time string. For a non-UTC clock, the core implementation uses `yyyy-MM-ddTHH:mm:ss`. Both normalization methods return empty or invalid values unchanged.

The standard shared MVC theme bundle loads Luxon and replaces the core implementations of `normalizeToString` and `normalizeToLocaleString`. When multiple time zones are supported, this Luxon implementation interprets the input in the configured IANA time zone, converts it to UTC and returns an ISO value ending in `Z`. The output can include milliseconds (for example, `2026-07-17T08:30:00.000Z`), so do not require an exact string length when consuming it.

If an application uses the core scripts without the shared theme's Luxon contributor, the fallback implementation produces a `Z`-suffixed transport value by detecting a numeric browser offset. It is not a full IANA time-zone conversion and does not account for fractional-hour offsets or an offset change between the current date and the input date. Include the Luxon contributor when those cases must be handled.

`normalizeToLocaleString` accepts standard `Intl.DateTimeFormat` options. When no options are supplied, it uses `abp.clock.toLocaleStringOptions`. The default options include the numeric year, long month, numeric day, hour, minute and second. You can replace them to define application-wide display defaults:

````js
abp.clock.toLocaleStringOptions = {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit'
};
````

## Browser Time-Zone Cookie

After application configuration is initialized, ABP writes the browser's time-zone name to the `__timezone` cookie when multiple time zones are supported. Set the following flag before configuration initialization to disable this behavior:

````js
abp.clock.trySetBrowserTimeZoneToCookie = false;
````

Use `abp.clock.browserTimeZone()` to read the browser time zone or `abp.clock.setBrowserTimeZoneToCookie()` to refresh the cookie explicitly.
