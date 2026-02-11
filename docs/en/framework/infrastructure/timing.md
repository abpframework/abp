```json
//[doc-seo]
{
    "Description": "Learn how ABP simplifies handling time and time zones for global applications with built-in services like IClock for accurate date and time management."
}
```

# Timing

Working with times & [time zones](https://en.wikipedia.org/wiki/Time_zone) is always tricky, especially if you need to build a **global system** that is used by users in **different time zones**.

ABP provides a basic infrastructure to make it easy and handle automatically wherever possible. This document covers the ABP services and systems related to time and time zones.

> If you are creating a local application that runs in a single time zone region, you may not need all these systems. But even in this case, it is suggested to use the `IClock` service introduced in this document.

## IClock

`DateTime.Now` returns a `DateTime` object with the **local date & time of the server**. A `DateTime` object **doesn't store the time zone information**. So, you can not know the **absolute date & time** stored in this object. You can only make **assumptions**, like assuming that it was created in UTC+05 time zone. Things especially get complicated when you save this value to a database and read it later, or send it to a client in a **different time zone**.

One solution to this problem is always using `DateTime.UtcNow` and assuming all `DateTime` objects as UTC time. In this way, you can convert it to the time zone of the target client when needed.

`IClock` provides an abstraction while getting the current time, so you can control the kind of the datetime (UTC or local) in a single point in your application.

**Example: Getting the current time**

````csharp
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly IClock _clock;

        public MyService(IClock clock)
        {
            _clock = clock;
        }

        public void Foo()
        {
            //Get the current time!
            var now = _clock.Now;
        }
    }
}
````

* Inject the `IClock` service when you need to get the current time. Common base classes (like ApplicationService) already inject it and provide it as a base property - so, you can directly use it as `Clock`.
* Use the `Now` property to get the current time.

> Most of the time, `IClock` is the only service you need to know and use in your application.

### Clock Options

`AbpClockOptions` is the [options](../fundamentals/options.md) class that is used to set the clock kind.

**Example: Use UTC Clock**

````csharp
Configure<AbpClockOptions>(options =>
{
    options.Kind = DateTimeKind.Utc;
});
````

Write this inside of the `ConfigureServices` method of your [module](../architecture/modularity/basics.md).

> The default `Kind` is `Unspecified`, which effectively disables the **Clock** functionality. Either make it `Utc` or `Local` if you want to get the benefit of the Clock system.
### DateTime Normalization

Another important function of the `IClock` is to normalize `DateTime` objects.

**Example usage:**

````csharp
DateTime dateTime = ...; //Get from somewhere
var normalizedDateTime = Clock.Normalize(dateTime)
````

`Normalize` method works as described below:

* Converts the given `DateTime` to the UTC (by using the `DateTime.ToUniversalTime()` method) if the current Clock is UTC and the given `DateTime` is local.
* Converts the given `DateTime` to the local (by using the `DateTime.ToLocalTime()` method) if the current Clock is local and the given `DateTime` is UTC.
* Sets `Kind` of the given `DateTime` (using the `DateTime.SpecifyKind(...)` method) to the `Kind` of the current Clock if given `DateTime`'s `Kind` is `Unspecified`.

`Normalize` method is used by the ABP when it gets a `DateTime` that is not created by `IClock.Now` and may not be compatible with the current Clock type. Examples;

* `DateTime` type binding in the ASP.NET Core MVC model binding.
* Saving data to and reading data from database via [Entity Framework Core](../data/entity-framework-core).
* Working with `DateTime` objects on [JSON deserialization](../../json-serialization.md).

#### DisableDateTimeNormalization Attribute

`DisableDateTimeNormalization` attribute can be used to disable the normalization operation for desired classes or properties.

### DateTime Converter Between UTC and User's Time Zone

#### Convert given UTC to user's time zone.

`DateTime ConvertToUserTime(DateTime utcDateTime)` and `DateTimeOffset ConvertToUserTime(DateTimeOffset dateTimeOffset)` methods convert given UTC `DateTime` or `DateTimeOffset` to the user's time zone.

> If `SupportsMultipleTimezone` is `false` or `dateTime.Kind` is not `Utc` or these is no timezone setting, it returns the given `DateTime` or `DateTimeOffset` without any changes.

**Example:**

If user's `TimeZone Setting` is `Europe/Istanbul`

````csharp
// 2025-03-01T05:30:00Z
var utcTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Utc);

var userTime = Clock.ConvertToUserTime(utcTime);

// Europe/Istanbul has 3 hours difference with UTC. So, the result will be 3 hours later.
userTime.Kind.ShouldBe(DateTimeKind.Unspecified);
userTime.ToString("O").ShouldBe("2025-03-01T08:30:00");
````

````csharp
// 2025-03-01T05:30:00Z
var utcTime = new DateTimeOffset(new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Utc), TimeSpan.Zero);

var userTime = Clock.ConvertToUserTime(utcTime);

// Europe/Istanbul has 3 hours difference with UTC. So, the result will be 3 hours later.
userTime.Offset.ShouldBe(TimeSpan.FromHours(3));
userTime.ToString("O").ShouldBe("2025-03-01T08:30:00.0000000+03:00");
````

#### Converts given user's DateTime to UTC

`DateTime ConvertToUtc(DateTime dateTime)` method convert given user's `DateTime` to UTC.

> If `SupportsMultipleTimezone` is `false` or `dateTime.Kind` is `Utc` or these is no timezone setting, it returns the given `DateTime` without any changes.

**Example:**

If user's `TimeZone Setting` is `Europe/Istanbul`

````csharp
// 2025-03-01T05:30:00
var userTime = new DateTime(2025, 3, 1, 5, 30, 0, DateTimeKind.Unspecified); //Same as Local

var utcTime = Clock.ConvertToUtc(userTime);

// Europe/Istanbul has 3 hours difference with UTC. So, the result will be 3 hours earlier.
utcTime.Kind.ShouldBe(DateTimeKind.Utc);
utcTime.ToString("O").ShouldBe("2025-03-01T02:30:00.0000000Z");
````

### Other IClock Properties

In addition to the `Now`, `IClock` service has the following properties:

* `Kind`: Returns a `DateTimeKind` for the currently used clock type (`DateTimeKind.Utc`, `DateTimeKind.Local` or `DateTimeKind.Unspecified`).
* `SupportsMultipleTimezone`: Returns `true` if the currently used clock is UTC.

## Time Zones

This section covers the ABP infrastructure related to managing time zones.

### TimeZone Setting

ABP defines **a setting**, named `Abp.Timing.TimeZone`, that can be used to set and get the time zone for a user, [tenant](../architecture/multi-tenancy) or globally for the application. The default value is empty, which means the application will use the server's time zone.

You can change your host/tenant global time zone in the [Settings Management UI](../../modules/setting-management#setting-management-ui)

The [Account Pro Module](../../modules/account-pro#Time-Zone-Setting) supports user to set their own time zone in the account settings page.

See the [setting documentation](../infrastructure/settings.md) to learn more about the setting system.

### UseAbpTimeZone Middleware

The `app.UseAbpTimeZone()` middleware is used to set the time zone for the current request.

    *  It will get timezone from settings, the order is `User` -> `Tenant` -> `Application/Global`.
    *  If current request is anonymous, it will get timezone from the request header/cookie/form/query string. the key is `__timezone`.

> If you want to get current timezone, you can inject `ICurrentTimezoneProvider` service.
> Please add this middleware after authentication.

### ITimezoneProvider

`ITimezoneProvider` is a service to simply convert [Windows Time Zone Id](https://support.microsoft.com/en-us/help/973627/microsoft-time-zone-index-values) values to [Iana Time Zone Name](https://www.iana.org/time-zones) values and vice verse. It also provides methods to get the list of these time zones and get a `TimeZoneInfo` with a given name.

It has been implemented using the [TimeZoneConverter](https://github.com/mj1856/TimeZoneConverter) library.
