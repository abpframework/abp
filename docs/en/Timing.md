# Timing

Working with times & [time zones](https://en.wikipedia.org/wiki/Time_zone) is always tricky, especially if you need to build a **global system** that is used by users in **different time zones**.

ABP provides a basic infrastructure to make it easy and handle automatically wherever possible. This document covers the ABP Framework services and systems related to time and time zones.

> If you are creating a local application that runs in a single time zone region, you may not need all these systems. But even in this case, it is suggested to use the `IClock` service introduced in this document.

## IClock

`DateTime.Now` returns a `DateTime` object with the **local date & time of the server**. A `DateTime` object **doesn't store the time zone information**. So, you can not know the **absolute date & time** stored in this object. You can only make **assumptions**, like assuming that it was created in UTC+05 time zone. The things especially gets complicated when you save this value to a database and read later, or send it to a client in a **different time zone**.

One solution to this problem is always use `DateTime.UtcNow` and assume all `DateTime` objects as UTC time. In this way, you can convert it to the time zone of the target client when needed.

`IClock` provides an abstraction while getting the current time, so you can control the kind of the date time (UTC or local) in a single point in your application.

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

* Inject the `IClock` service when you need to get the current time. Common base classes (like ApplicationService) already injects it and provides as a base property - so, you can directly use as `Clock`.
* Use the `Now` property to get the current time.

> Most of the times, `IClock` is the only service you need to know and use in your application.

### Clock Options

`AbpClockOptions` is the [options](Options.md) class that used to set the clock kind.

**Example: Use UTC Clock**

````csharp
Configure<AbpClockOptions>(options =>
{
    options.Kind = DateTimeKind.Utc;
});
````

Write this inside the `ConfigureServices` method of your [module](Module-Development-Basics.md).

> Default `Kind` is `Unspecified`, that actually make the Clock as it doesn't exists at all. Either make it `Utc` or `Local` if you want to get benefit of the Clock system.

### DateTime Normalization

Other important function of the `IClock` is to normalize `DateTime` objects.

**Example usage:**

````csharp
DateTime dateTime = ...; //Get from somewhere
var normalizedDateTime = Clock.Normalize(dateTime)
````

`Normalize` method works as described below:

* Converts the given `DateTime` to the UTC (by using the `DateTime.ToUniversalTime()` method) if current Clock is UTC and given `DateTime` is local.
* Converts the given `DateTime` to the local (by using the `DateTime.ToLocalTime()` method) if current Clock is local and given `DateTime` is UTC.
* Sets `Kind` of the given `DateTime` (using the `DateTime.SpecifyKind(...)` method) to the `Kind` of the current Clock if given `DateTime`'s `Kind` is `Unspecified`.

`Normalize` method is used by the ABP Framework when the it gets a `DateTime` that is not created by `IClock.Now` and may not be compatible with the current Clock type. Examples;

* `DateTime` type binding in the ASP.NET Core MVC model binding.
* Saving data to and reading data from database via [Entity Framework Core](Entity-Framework-Core.md).
* Working with `DateTime` objects on [JSON deserialization](Json-Serialization.md).

#### DisableDateTimeNormalization Attribute

`DisableDateTimeNormalization` attribute can be used to disable the normalization operation for desired classes or properties.

### Other IClock Properties

In addition to the `Now`, `IClock` service has the following properties:

* `Kind`: Returns a `DateTimeKind` for the currently used clock type (`DateTimeKind.Utc`, `DateTimeKind.Local` or `DateTimeKind.Unspecified`).
* `SupportsMultipleTimezone`: Returns `true` if currently used clock is UTC.

## Time Zones

This section covers the ABP Framework infrastructure related to managing time zones.

### TimeZone Setting

ABP Framework defines **a setting**, named `Abp.Timing.Timezone`, that can be used to set and get the time zone for a user, [tenant](Multi-Tenancy.md) or globally for the application. The default value is `UTC`.

See the [setting documentation](Settings.md) to learn more about the setting system.

### ITimezoneProvider

`ITimezoneProvider` is a service to simple convert [Windows Time Zone Id](https://support.microsoft.com/en-us/help/973627/microsoft-time-zone-index-values) values to [Iana Time Zone Name](https://www.iana.org/time-zones) values and vice verse. It also provides methods to get list of these time zones and get a `TimeZoneInfo` with a given name.

It has been implemented using the [TimeZoneConverter](https://github.com/mj1856/TimeZoneConverter) library.
