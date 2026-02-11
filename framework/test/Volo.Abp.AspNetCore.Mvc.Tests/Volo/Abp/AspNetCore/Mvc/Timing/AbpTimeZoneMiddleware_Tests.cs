using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Timing;

public class AbpTimeZoneMiddleware_Tests : AspNetCoreMvcTestBase
{
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;
    private readonly ITimezoneProvider _timezoneProvider;
    public AbpTimeZoneMiddleware_Tests()
    {
        _currentTimezoneProvider = GetRequiredService<ICurrentTimezoneProvider>();
        _timezoneProvider = GetRequiredService<ITimezoneProvider>();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AbpClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
    }

    [Fact]
    public async Task Should_Override_TimeZone_Setting_By_Request()
    {
        using (_currentTimezoneProvider.Change("UTC"))
        {
            var result = await Client.GetStringAsync("api/timing-test");
            result.ShouldBe(GetLocalTimeZone());
        }

        // Query string
        using (_currentTimezoneProvider.Change("UTC"))
        {
            var result = await Client.GetStringAsync("api/timing-test?__timezone=Europe/Istanbul");
            result.ShouldBe("Europe/Istanbul");
        }

        // Header
        using (_currentTimezoneProvider.Change("UTC"))
        {
            Client.DefaultRequestHeaders.Add("__timezone", "Asia/Shanghai");
            var result = await Client.GetStringAsync("api/timing-test");
            result.ShouldBe("Asia/Shanghai");
        }

        // Form
        using (_currentTimezoneProvider.Change("UTC"))
        {
            Client.DefaultRequestHeaders.Remove("__timezone");
            var result = await Client.PostAsync("api/timing-test", new FormUrlEncodedContent(new[] {new KeyValuePair<string, string>("__timezone", "Europe/Germany")}));
            (await result.Content.ReadAsStringAsync()).ShouldBe("Europe/Germany");
        }

        // Cookie
        using (_currentTimezoneProvider.Change("UTC"))
        {
            Client.DefaultRequestHeaders.Remove("__timezone");
            Client.DefaultRequestHeaders.Add("Cookie", "__timezone=Europe/Istanbul");
            var result = await Client.GetStringAsync("api/timing-test");
            result.ShouldBe("Europe/Istanbul");
        }
    }

    private string GetLocalTimeZone()
    {
        if (TimeZoneInfo.Local.HasIanaId)
        {
            return TimeZoneInfo.Local.Id;
        }

        return TimeZoneInfo.TryConvertWindowsIdToIanaId(TimeZoneInfo.Local.Id, out var ianaName)
            ? ianaName
            : null;
    }
}


public class AbpTimeZoneMiddleware_With_SettingValue_Tests : AspNetCoreMvcTestBase
{
    private readonly ICurrentTimezoneProvider _currentTimezoneProvider;
    public AbpTimeZoneMiddleware_With_SettingValue_Tests()
    {
        _currentTimezoneProvider = GetRequiredService<ICurrentTimezoneProvider>();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        var settingStore = Substitute.For<ISettingProvider>();
        settingStore.GetOrNullAsync(TimingSettingNames.TimeZone).Returns("Asia/Shanghai");
        services.AddSingleton(settingStore);

        services.Configure<AbpClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
    }

    [Fact]
    public async Task Should_Not_Override_TimeZone_Setting_By_Request()
    {
        using (_currentTimezoneProvider.Change("UTC"))
        {
            var result = await Client.GetStringAsync("api/timing-test");
            result.ShouldBe("Asia/Shanghai");
        }

        // Query string
        using (_currentTimezoneProvider.Change("UTC"))
        {
            var result = await Client.GetStringAsync("api/timing-test?__timezone=Europe/Istanbul");
            result.ShouldBe("Asia/Shanghai");
        }

        // Header
        using (_currentTimezoneProvider.Change("UTC"))
        {
            Client.DefaultRequestHeaders.Add("__timezone", "Europe/Istanbul");
            var result = await Client.GetStringAsync("api/timing-test");
            result.ShouldBe("Asia/Shanghai");
        }

        // Form
        using (_currentTimezoneProvider.Change("UTC"))
        {
            Client.DefaultRequestHeaders.Remove("__timezone");
            var result = await Client.PostAsync("api/timing-test", new FormUrlEncodedContent(new[] {new KeyValuePair<string, string>("__timezone", "Europe/Istanbul")}));
            (await result.Content.ReadAsStringAsync()).ShouldBe("Asia/Shanghai");
        }

        // Cookie
        using (_currentTimezoneProvider.Change("UTC"))
        {
            Client.DefaultRequestHeaders.Remove("__timezone");
            Client.DefaultRequestHeaders.Add("Cookie", "__timezone=Europe/Istanbul");
            var result = await Client.GetStringAsync("api/timing-test");
            result.ShouldBe("Asia/Shanghai");
        }
    }
}
